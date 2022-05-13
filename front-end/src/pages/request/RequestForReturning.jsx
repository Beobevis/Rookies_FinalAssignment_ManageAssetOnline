import {Table, Modal, Button, Dropdown, Menu, Input, Col, Row, DatePicker} from 'antd';
import {CheckOutlined, CloseOutlined, FilterOutlined, LoadingOutlined} from "@ant-design/icons";
import axios from 'axios';
import React, {useEffect, useState} from 'react';
import "antd/dist/antd.css";
import moment from "moment";

// import {useParams} from 'react-router-dom';
export default function RequestForReturningPage() {
    const [data, setData] = useState([]);
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [isModalCancelVisible, setIsModalCancelVisible] = useState(false);
    const [idCompleted, setIdCompleted] = useState();
    const [state, setState] = useState("state");
    const [searchText, setSearchText] = useState("");
    const [date, setDate] = useState();
    const showModal = () => {
        setIsModalVisible(true);

    };

    const handleCheckId = (id) => {
        setIdCompleted(id)
    }

    const handleOk = () => {
        setIsModalVisible(false);

        axios.put(`${process.env.REACT_APP_UNSPLASH_REQUEST_FOR_RETURNING_PUT}=${idCompleted}`)
            .then(() => {

                window.location.reload();
                setIdCompleted(null)
            }).catch(() => {

        })


    };


    const handleCancel = () => {
        setIsModalVisible(false);
    };


    const showModalDelete = () => {
        setIsModalCancelVisible(true);

    };
    const handleCheckDeleteId = (id) => {

        setIdCompleted(id)
    }
    const handleDeleteOk = () => {
        setIsModalCancelVisible(false);
        axios
            .delete(`${process.env.REACT_APP_UNSPLASH_REQUEST_FOR_RETURNING_DELETE}=${idCompleted}`)
            .then((res) => {
                setIdCompleted(null)

                window.location.reload();
            }).catch((error) => {

        })
    }
    const handleCancelModal = () => {
        setIsModalCancelVisible(false);
    };

    useEffect(() => {
        axios
            .get(`${process.env.REACT_APP_UNSPLASH_REQUEST_FOR_RETURNING_URL}`, {})
            .then(function (response) {


                let respData = response.data;

                respData.forEach((element) => {
                    element.state = element.state === 'WaitingForReturn' ? 'Waiting For Returning' : element.state;
                    element.returnedDate = element.returnedDate === "0001-01-01T00:00:00" ? "" : moment(new Date(element.returnedDate).toLocaleDateString("en-US")).format('DD/MM/YYYY');
                    element.assignedDate = moment(new Date(element.assignedDate).toLocaleDateString("en-US")).format('DD/MM/YYYY');
                    element.action = [

                        <Button
                            className="buttonState"
                                disabled={element.state === 'Completed'}
                                onClick={() => {
                                    showModal()
                                    handleCheckId(element.id)
                                }}>
                            <CheckOutlined
                                style={{ color: "red", fontSize: "13px"}}

                            />
                        </Button>,

                        <Button
                            className="buttonState"
                            disabled={element.state === 'Completed'}
                            onClick={() => {
                                showModalDelete()
                                handleCheckDeleteId(element.id)
                            }}
                        >
                            <CloseOutlined style={{fontSize: "13px"}}
                            />
                        </Button>
                    ];
                });

                setData(respData);
            })
            .catch(() => {

            });
    }, []);

    const dataBytype = state === "state" ? data : data.filter((u) => u.state === state);
    const filterData =
        searchText === ""
            ? dataBytype
            : dataBytype.filter(
                (u) =>
                ( u.assetName.toLowerCase()).replace(/\s+/g, '').includes(searchText.toLowerCase().replace(/\s+/g, '')) ||
                    u.assetCode.toLowerCase().includes(searchText.toLowerCase()) ||
                    u.requestedBy.toLowerCase().includes(searchText.toLowerCase())
            );
    const finalData =  date == null ? filterData  :   filterData.filter(u=>u.assignedDate ===  moment(new Date(date).toLocaleDateString("en-US")).format('DD/MM/YYYY'))

    const columns = [

        {
            title: "No.",
            dataIndex: "id",
            key: "id",
            render: (text, record, id) => id + 1,
        },
        {
            title: "Asset Code",
            dataIndex: "assetCode",
            key: "assetCode",
            sorter: (a, b) => {
                if (a.assetCode > b.assetCode) {
                    return -1;
                }
                if (b.assetCode > a.assetCode) {
                    return 1;
                }
                return 0;
            },
        },
        {
            title: "Asset Name",
            dataIndex: "assetName",
            key: "assetName",
            sorter: (a, b) => {
                if (a.assetName > b.assetName) {
                    return -1;
                }
                if (b.assetName > a.assetName) {
                    return 1;
                }
                return 0;
            },
        },
        {
            title: "Requested By",
            dataIndex: "requestedBy",
            key: "requestedBy",
            sorter: (a, b) => {
                if (a.requestedById > b.requestedById) {
                    return -1;
                }
                if (b.requestedById > a.requestedById) {
                    return 1;
                }
                return 0;
            },
        },
        {
            title: "Assigned Date",
            dataIndex: "assignedDate",
            key: "assignedDate",
            sorter: (a, b) => {
                if (a.assignedDate > b.assignedDate) {
                    return -1;
                }
                if (b.assignedDate > a.assignedDate) {
                    return 1;
                }
                return 0;
            },
        },
        {
            title: "Accepted By",
            dataIndex: "acceptedBy",
            key: "acceptedBy",
            sorter: (a, b) => {
                if (a.acceptedById > b.acceptedById) {
                    return -1;
                }
                if (b.acceptedById > a.acceptedById) {
                    return 1;
                }
                return 0;
            },
        },
        {
            title: "Returned Date",
            dataIndex: "returnedDate",
            key: "returnedDate",
            sorter: (a, b) => {
                if (a.returnedDate > b.returnedDate) {
                    return -1;
                }
                if (b.returnedDate > a.returnedDate) {
                    return 1;
                }
                return 0;
            },
        },
        {
            title: "State",
            dataIndex: "state",
            key: "state",
            sorter: (a, b) => {
                if (a.state > b.state) {
                    return -1;
                }
                if (b.state > a.state) {
                    return 1;
                }
                return 0;
            },
        },
        {
            title: "",
            dataIndex: "action",
            key: "action",
        },
    ];

    return (
        <>
            <Modal
                closable={false}
                title="Are You Sure?" visible={isModalVisible} okText="Yes" cancelText="No" onOk={handleOk}
                   onCancel={handleCancel}
                   footer={[
                       <div style={{textAlign: "left"}}>
                           <Button style={{}} key="Yes" onClick={handleOk} className = "buttonSave">Yes</Button>
                           <Button  key="No" onClick={handleCancel} className="buttonCancel">No</Button>
                       </div>
                   ]}>
                <p>Do you want to mark this returning request as "Completed"?</p>
            </Modal>

            <Modal
                closable={false}
                title="Are You Sure?" visible={isModalCancelVisible} okText="Yes" cancelText="No"
                   onOk={handleDeleteOk} onCancel={handleCancelModal}
                   footer={[
                       <div style={{textAlign: "left"}}>
                           <Button style={{}} key="Yes" onClick={handleDeleteOk} className = "buttonSave">Yes</Button>
                           <Button  key="No" onClick={handleCancelModal} className="buttonCancel">No</Button>
                       </div>
                   ]}>
                <p>Do you want to cancel this returning request?</p>
            </Modal>
            <p style={{
                display: 'block',
                fontSize: "20px",
                margin: '0 auto',
                textAlign: 'left',
                color: ' red',
                fontWeight: 'bold',
                paddingBottom: "20px"
            }}>Request List</p>
            <Row gutter={45} style={{marginBottom: "30px"}}>
                <Col xs={8} sm={8} md={8} lg={8} xl={8} xxl={7}>
                    <Dropdown.Button
                        style={{float: 'left'}}
                        placement="bottom"
                        icon={<FilterOutlined/>}
                        overlay={
                            <Menu>
                                <Menu.Item
                                    value="Completed"
                                    onClick={() => {
                                        setState("Completed");
                                    }}
                                >
                                    Completed
                                </Menu.Item>
                                <Menu.Item
                                    value="Waiting For Returning"
                                    onClick={() => {
                                        setState("Waiting For Returning");
                                    }}
                                >

                                    Waiting For Returning
                                </Menu.Item>

                                <Menu.Item

                                    onClick={() => {
                                        setState("state");
                                    }}
                                >
                                    All
                                </Menu.Item>
                            </Menu>
                        }
                    >
                        {state}
                    </Dropdown.Button>
                </Col>
                <Col xs={8} sm={8} md={6} lg={6} xl={6} xxl={6}>
                    <DatePicker placeholder="Returned Date" format="DD/MM/YYYY" allowClear={true} onChange={(date) => {
                        setDate(date);

                    }}/>
                </Col>
                <Col xs={8} sm={8} md={6} lg={6} xl={6} xxl={6}>
                    <Input.Search
                        maxLength={255}
                        allowClear
                        onSearch={(e) => {

                            setSearchText(e.replace(/ /g, ''))
                        }}
                        onEnter={(e) => {
                            setSearchText(e.replace(/ /g, ''))

                        }}

                    />
                </Col>

            </Row>
            {data.length === 0 ? <Table
                    loading={{indicator: <LoadingOutlined style={{fontSize: "60px", color: "red"}}/>, spinning: true}}/> :
                <Table
                    size="large"
                    columns={columns}
                    dataSource={finalData}
                >
                </Table>
            }
        </>


    );

}