import React, { useState, useEffect } from 'react';
import { 
  Table, Button, Space, Tag, Modal, Form, Input, 
  Select, message, Card, Typography, Popconfirm, Radio 
} from 'antd';
import { 
  PlusOutlined, EditOutlined, DeleteOutlined, 
  CheckCircleOutlined, SyncOutlined 
} from '@ant-design/icons';
import { roomApi } from '../../api/roomApi'; 

const { Title, Text } = Typography;
const { Option } = Select;
const { Search } = Input;

const RoomManagement = () => {
  const [rooms, setRooms] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [filterStatus, setFilterStatus] = useState('All');
  const [editingRoom, setEditingRoom] = useState(null); 
  const [form] = Form.useForm();

  // --- 1. HÚT DATA TỪ SERVER ---
  const fetchRooms = async () => {
    setLoading(true);
    try {
      const response = await roomApi.getRooms();
      setRooms(response.data);
    } catch (error) {
      console.error("Lỗi lấy dữ liệu phòng:", error);
      message.error("Lỗi kết nối Backend. Không thể lấy dữ liệu!");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRooms();
  }, []);

  // --- 2. LOGIC LỌC DỮ LIỆU TẠI CHỖ (SEARCH & FILTER) ---
  const filteredRooms = rooms.filter(room => {
    const matchesSearch = room.roomNumber.toLowerCase().includes(searchText.toLowerCase());
    const matchesStatus = filterStatus === 'All' || room.status === filterStatus;
    return matchesSearch && matchesStatus;
  });

  // --- 3. CÁC HÀM XỬ LÝ SỰ KIỆN ---
  const handleAddNew = () => {
    setEditingRoom(null);
    form.resetFields();
    setIsModalVisible(true);
  };

  const handleEdit = (record) => {
    setEditingRoom(record);
    form.setFieldsValue(record);
    setIsModalVisible(true);
  };

  const handleSave = async () => {
    try {
      const values = await form.validateFields(); 
      setLoading(true);

      const payload = {
        id: editingRoom ? editingRoom.id : 0,
        roomNumber: values.roomNumber,
        floor: Number(values.floor),
        roomTypeId: Number(values.roomTypeId || 1),
        status: values.status,
        cleaningStatus: values.cleaningStatus || 'Clean'
      };

      if (editingRoom) {
        await roomApi.updateRoom(editingRoom.id, payload);
        message.success(`Cập nhật phòng ${values.roomNumber} thành công!`);
      } else {
        await roomApi.createRoom(payload);
        message.success(`Thêm phòng ${values.roomNumber} thành công!`);
      }

      setIsModalVisible(false);
      fetchRooms();
    } catch (error) {
      console.log("❌ LỖI TỪ C#:", error.response?.data);
      const msg = error.response?.data?.message || "Có lỗi xảy ra!";
      message.error(msg);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    setLoading(true);
    try {
      await roomApi.deleteRoom(id);
      message.success("Đã xóa phòng thành công!");
      fetchRooms();
    } catch (error) {
      const msg = error.response?.data?.message || "Lỗi hệ thống!";
      message.error(msg, 5);
    } finally {
      setLoading(false);
    }
  };

  // --- 4. CẤU HÌNH CỘT CỦA BẢNG ---
  const columns = [
    { title: 'ID', dataIndex: 'id', key: 'id', width: 60 },
    { title: 'Số Phòng', dataIndex: 'roomNumber', key: 'roomNumber', render: text => <b>{text}</b> },
    { title: 'Tầng', dataIndex: 'floor', key: 'floor' },
    {
      title: 'Trạng Thái',
      dataIndex: 'status',
      key: 'status',
      render: status => {
        let color = status === 'Available' ? 'green' : status === 'Occupied' ? 'red' : 'orange';
        return <Tag color={color}>{status || 'N/A'}</Tag>;
      },
    },
    {
      title: 'Dọn Dẹp',
      dataIndex: 'cleaningStatus',
      key: 'cleaningStatus',
      render: status => {
        let color = status === 'Clean' ? 'cyan' : 'warning';
        let icon = status === 'Clean' ? <CheckCircleOutlined /> : <SyncOutlined spin />;
        return <Tag icon={icon} color={color}>{status || 'N/A'}</Tag>;
      },
    },
    {
      title: 'Thao tác',
      key: 'action',
      render: (_, record) => (
        <Space size="middle">
          <Button type="primary" ghost icon={<EditOutlined />} size="small" onClick={() => handleEdit(record)}>Sửa</Button>
          <Popconfirm
            title="Xóa phòng này?"
            onConfirm={() => handleDelete(record.id)}
            okText="Xóa" cancelText="Hủy" okButtonProps={{ danger: true }}
          >
            <Button type="primary" danger icon={<DeleteOutlined />} size="small">Xóa</Button>
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <Card style={{ margin: 24, borderRadius: 12, boxShadow: '0 4px 20px rgba(0,0,0,0.08)' }}>
      {/* HEADER SECTION */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 24 }}>
        <div>
          <Title level={3} style={{ margin: 0 }}>Quản Lý Phòng</Title>
          <Text type="secondary">Tìm kiếm, lọc và cập nhật thông tin phòng khách sạn</Text>
        </div>
        <Button type="primary" size="large" icon={<PlusOutlined />} onClick={handleAddNew} style={{ borderRadius: 8 }}>
          Thêm Phòng Mới
        </Button>
      </div>

      {/* FILTER & SEARCH SECTION */}
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20, flexWrap: 'wrap', gap: '10px' }}>
        <Radio.Group 
          value={filterStatus} 
          onChange={(e) => setFilterStatus(e.target.value)}
          optionType="button"
          buttonStyle="solid"
        >
          <Radio.Button value="All">Tất cả ({rooms.length})</Radio.Button>
          <Radio.Button value="Available">Trống</Radio.Button>
          <Radio.Button value="Occupied">Đang ở</Radio.Button>
          <Radio.Button value="Maintenance">Bảo trì</Radio.Button>
        </Radio.Group>

        <Search
          placeholder="Tìm số phòng... (VD: 101)"
          allowClear
          onSearch={(val) => setSearchText(val)}
          onChange={(e) => setSearchText(e.target.value)}
          style={{ width: 300 }}
        />
      </div>

      {/* MAIN TABLE */}
      <Table 
        columns={columns} 
        dataSource={filteredRooms} 
        rowKey="id" 
        loading={loading}
        pagination={{ pageSize: 8, showTotal: (total) => `Tổng cộng ${total} phòng` }}
      />

      {/* ADD/EDIT MODAL */}
      <Modal
        title={editingRoom ? "📝 Sửa Thông Tin Phòng" : "✨ Thêm Phòng Mới"}
        open={isModalVisible}
        onCancel={() => setIsModalVisible(false)}
        onOk={handleSave}
        confirmLoading={loading}
        okText="Lưu lại"
        cancelText="Hủy"
        centered
      >
        <Form form={form} layout="vertical" style={{ marginTop: 15 }}>
          <Form.Item label="Số Phòng" name="roomNumber" rules={[{ required: true, message: 'Nhập số phòng đi ní!' }]}>
            <Input placeholder="VD: 101, VILLA-1" />
          </Form.Item>
          
          <Space style={{ display: 'flex' }} align="baseline">
            <Form.Item label="Tầng" name="floor" rules={[{ required: true, message: 'Nhập tầng!' }]}>
              <Input type="number" placeholder="VD: 1" style={{ width: 220 }} />
            </Form.Item>
            <Form.Item label="Mã Loại Phòng (ID)" name="roomTypeId" rules={[{ required: true }]}>
              <Input type="number" placeholder="VD: 1" style={{ width: 220 }} />
            </Form.Item>
          </Space>

          <Form.Item label="Trạng Thái" name="status" initialValue="Available">
            <Select>
              <Option value="Available">Available (Trống)</Option>
              <Option value="Occupied">Occupied (Có khách)</Option>
              <Option value="Maintenance">Maintenance (Bảo trì)</Option>
            </Select>
          </Form.Item>

          <Form.Item label="Tình trạng Dọn Dẹp" name="cleaningStatus" initialValue="Clean">
            <Select>
              <Option value="Clean">Clean (Sạch sẽ)</Option>
              <Option value="Dirty">Dirty (Chưa dọn)</Option>
              <Option value="Inspecting">Inspecting (Đang kiểm tra)</Option>
            </Select>
          </Form.Item>
        </Form>
      </Modal>
    </Card>
  );
};

export default RoomManagement;