import React, {useState, useEffect} from "react";
import {Table, Input, Button, Menu, Dropdown, Row, Col, Modal, Form, DatePicker} from "antd";
import {
    FilterOutlined,
    EditFilled,
    CloseCircleOutlined,
    LoadingOutlined,
    ReloadOutlined
} from "@ant-design/icons";
import {Link} from "react-router-dom";
import axios from "axios";
import "antd/dist/antd.css";
import moment from "moment";

export default function ManageAssignment() {
    const [data, setData] = useState([]);
    const [searchText, setSearchText] = useState("");
    const [page, setPage] = useState(1);
    const [modal, setModal] = useState({
        isOpen: false,
        data: {},
    });
    const [isModalReturnVisible, setIsModalReturnVisible] = useState(false);
    const [pageSize, setPageSize] = useState(10);
    const [type, setType] = useState("");
    const [date, setDate] = useState()
    const [isModalCancelVisible, setIsModalCancelVisible] = useState(false);
    const [idCompleted, setIdCompleted] = useState();

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
            title: "Assigned To",
            dataIndex: "assignTo",
            key: "assignTo",
            sorter: (a, b) => {
                if (a.assignTo > b.assignTo) {
                    return -1;
                }
                if (b.assignTo > a.assignTo) {
                    return 1;
                }
                return 0;
            },
        },
        {
            title: "Assigned By",
            dataIndex: "assignedBy",
            key: "assignedBy",
            sorter: (a, b) => {
                if (a.assignedBy > b.assignedBy) {
                    return -1;
                }
                if (b.assignedBy > a.assignedBy) {
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
            title: "State",
            dataIndex: "state",
            key: "state",
            width: "100px",
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

    useEffect(() => {

        axios
            .get(`${process.env.REACT_APP_UNSPLASH_ASSIGNMENT}`)
            .then(function (response) {


                let respData = response.data;

                respData.forEach((assignment) => {
                    assignment.state = assignment.state === 'WaitingForAcceptance' ? 'Waiting For Acceptance' : assignment.state
                    assignment.assignedDate = moment(new Date(assignment.assignedDate).toLocaleDateString("en-US")).format('DD/MM/YYYY');
                    assignment.action = [
                        <Button
                            className="buttonState"
                            disabled={assignment.state === "Accepted" || assignment.state === "Declined"}
                        >
                            <Link style={{color: "green"}} to={`/editAssignment/${assignment.id}`} id="editButton">
                                <EditFilled style={{fontSize: "13px"}}/>
                            </Link>
                        </Button>,
                        <Button
                            style={{color: "#e30c18"}}
                            className="buttonState"
                            disabled={assignment.state === "Accepted"}
                            onClick={() => {
                                showModalDelete()
                                handleCheckDeleteId(assignment.id)
                            }}
                        >
                            <CloseCircleOutlined style={{fontSize: "13px"}}
                            />
                        </Button>,
                        <Button
                            className="buttonState"
                            disabled={assignment.state === "Waiting For Acceptance" || assignment.state === "Declined"}
                            onClick={() => {
                                showModalReturn()
                                handleCheckReturnId(assignment.id)
                            }}
                        >
                            <ReloadOutlined style={{color: "Blue", fontSize: "13px"}}
                            />
                        </Button>

                    ];
                });

                setData(respData.sort((a, b) => {
                    if (a.assetCode.trim().toLowerCase() > b.assetCode.trim().toLowerCase()) {
                        return 1;
                    }
                    if (b.assetCode.trim().toLowerCase() > a.assetCode.trim().toLowerCase()) {
                        return -1;
                    }
                    return 0;
                }));
            }, [])
            .catch(() => {

            });
    }, []);
    const showModalDelete = () => {
        setIsModalCancelVisible(true);

    };
    const handleCheckDeleteId = (id) => {

        setIdCompleted(id)
    }
    const handleDeleteOk = () => {
        setIsModalCancelVisible(false);
        axios
            .delete(`${process.env.REACT_APP_UNSPLASH_ASSIGNMENT}?id=${idCompleted}`)
            .then((res) => {
                setIdCompleted(null)

                window.location.reload();
            }).catch((error) => {

        })
    }
    const handleCancelModal = () => {
        setIsModalCancelVisible(false);
    };

    const dataBytype = type === "" ? data : data.filter((u) => u.state === type);

    const filterData =
        searchText === ""
            ? dataBytype
            : dataBytype.filter(
                (u) =>
                    (u.assetName.toLowerCase()).replace(/\s+/g, '').includes(searchText.toLowerCase().replace(/\s+/g, '')) ||
                    u.assetCode.toLowerCase().includes(searchText.toLowerCase()) ||
                    u.assignTo.toLowerCase().includes(searchText.toLowerCase())
            );

    const finalData = date == null ? filterData : filterData.filter(u => u.assignedDate === moment(new Date(date).toLocaleDateString("en-US")).format('DD/MM/YYYY'))

    const pagination = {
        current: page,
        pageSize: pageSize,
        total: filterData.length,
        pageSizeOptions: [5, 10, 15, 20],
        className: "ant-btn-dangerous",
        dangerous: true,
        onChange: (page, pageSize) => {
            setPage(page);
            setPageSize(pageSize);
        },
    };
    const showModalReturn = () => {
        setIsModalReturnVisible(true);
    }
    const handleCheckReturnId = (id) => {
        setIdCompleted(id)
    }
    const handleReturnOk = () => {
        setIsModalReturnVisible(false);
        axios
            .post(`${process.env.REACT_APP_UNSPLASH_RETURNREQUEST_ADMIN}/${idCompleted}`)
            .then(() => {
                setIdCompleted(null)

                window.location.reload();
            }).catch((error) => {

            alert(error.data.response.message)
        })
    }
    const handleCancelReturnModal = () => {
        setIsModalReturnVisible(false);
    };
    return (
        <>
            <Modal
                closable={false}
                title="Are You Sure?" visible={isModalReturnVisible} okText="Yes" cancelText="No" onOk={handleReturnOk}
                onCancel={handleCancelReturnModal}
                footer={[
                    <div style={{textAlign: "left"}}>
                        <Button style={{}} key="Yes" onClick={handleReturnOk} className="buttonSave">Yes</Button>
                        <Button style={{color: 'white'}} key="No" onClick={handleCancelReturnModal}
                                className="buttonCancel">No</Button>
                    </div>
                ]}>
                <p>Do you want to create a returning request for this asset?</p>
            </Modal>
            <Modal
                closable={false}
                title="Are You Sure?" visible={isModalCancelVisible} okText="Yes" cancelText="No" onOk={handleDeleteOk}
                onCancel={handleCancelModal}
                footer={[
                    <div style={{textAlign: "left"}}>
                        <Button style={{}} key="Yes" onClick={handleDeleteOk} className="buttonSave">Delete</Button>
                        <Button style={{color: 'white'}} key="No" onClick={handleCancelModal}
                                className="buttonCancel">Cancel</Button>
                    </div>
                ]}>
                <p>Do you want to delete this assignment?</p>
            </Modal>
            <p style={{
                display: 'block',
                fontSize: "20px",
                margin: '0 auto',
                textAlign: 'left',
                color: ' red',
                fontWeight: 'bold',
                paddingBottom: "20px"
            }}>Assignment List</p>
            <Row gutter={45} style={{marginBottom: "30px", display: "flex"}}>

                <Col xs={8} sm={8} md={7} lg={7} xl={3} xxl={5}>
                    <Dropdown.Button
                        placement="bottom"
                        icon={<FilterOutlined/>}
                        overlay={
                            <Menu>
                                <Menu.Item
                                    value="Accepted"
                                    onClick={() => {
                                        setType("Accepted");
                                    }}
                                >
                                    Accepted
                                </Menu.Item>
                                <Menu.Item
                                    value="Waiting For Acceptance"
                                    onClick={() => {
                                        setType("Waiting For Acceptance");
                                    }}
                                >
                                    Waiting For Acceptance
                                </Menu.Item>

                                <Menu.Item
                                    value="Declined"
                                    onClick={() => {
                                        setType("Declined");
                                    }}
                                >
                                    Declined
                                </Menu.Item>

                                <Menu.Item

                                    onClick={() => {
                                        setType("");
                                    }}
                                >
                                    All
                                </Menu.Item>
                            </Menu>
                        }
                    >
                        State
                    </Dropdown.Button>
                </Col>
                <Col xs={5} sm={5} md={5} lg={6} xl={6} xxl={6}>
                    <Form.Item initialValue="invalid">

                        <DatePicker placeholder="Assigned Date" format="DD/MM/YYYY" allowClear={true}
                                    onChange={(date) => {
                                        setDate(date);

                                    }}/>
                    </Form.Item>
                </Col>
                <Col xs={5} sm={5} md={5} lg={6} xl={6} xxl={6}>
                    <Input.Search

                        maxLength={255}
                        allowClear
                        onSearch={(e) => {
                            setPage(1)
                            setSearchText(e.replace(/ /g, ''))
                        }}
                        onEnter={(e) => {
                            setSearchText(e.replace(/ /g, ''))
                            setPage(1)
                        }}

                    />
                </Col>
                <Col xs={7} sm={7} md={6} lg={6} xl={6} xxl={6}>
                    <Button style={{background: "#e30c18", color: "white"}}>
                        <Link to="/createAssignment"> Create new Assignment</Link>
                    </Button>
                </Col>
            </Row>

            <Modal
                visible={modal.isOpen}
                title='Detail Assigment'
                onOk={() => {
                    setModal({...modal, isOpen: false});
                }}
                footer={[
                    <Button key="back"
                            style={{background: "#e30c18", color: "white"}}
                            onClick={() => {
                                setModal({...modal, isOpen: false});
                            }
                            }>Close</Button>
                ]}
                closable={false}

            >

                <table>
                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>Asset Code</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.assetCode}</td>
                    </tr>
                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>Asset Name</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.assetName}</td>
                    </tr>
                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>Assigned To</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.assignTo}</td>
                    </tr>
                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>Assigned By</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.assignedBy}</td>
                    </tr>

                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>Assigned Date</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.assignedDate}</td>
                    </tr>
                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>State</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.state}</td>
                    </tr>
                </table>


            </Modal>
            {data.length === 0 ? <Table
                    loading={{indicator: <LoadingOutlined style={{fontSize: "60px", color: "red"}}/>, spinning: true}}/> :
                <Table
                    rowKey="key"
                    columns={columns}
                    pagination={pagination}
                    dataSource={finalData}
                    onRow={(record) => {
                        return {
                            onClick: (e) => {

                                if (e.target.className === 'ant-table-cell ant-table-cell-row-hover') {
                                    setModal({
                                        ...modal, isOpen: true
                                        , data: {
                                            assetCode: record.assetCode,
                                            assetName: record.assetName,
                                            assignTo: record.assignTo,
                                            assignedBy: record.assignedBy,
                                            assignedDate: record.assignedDate,
                                            state: record.state,
                                        }
                                    });
                                } else if (e.target.className === 'ant-table-cell ant-table-column-sort ant-table-cell-row-hover') {
                                    setModal({
                                        ...modal, isOpen: true
                                        , data: {
                                            assetCode: record.assetCode,
                                            assetName: record.assetName,
                                            assignTo: record.assignTo,
                                            assignedBy: record.assignedBy,
                                            assignedDate: record.assignedDate,
                                            state: record.state,
                                        }
                                    });
                                } else {
                                    setModal({...modal, isOpen: false})

                                }


                            }
                        }
                    }}

                />
            }
        </>

    );
}
