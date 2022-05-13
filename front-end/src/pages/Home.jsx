import {Table, Modal, Button} from 'antd';
import axios from 'axios';
import React, {useEffect, useState} from 'react';
import {CheckOutlined, CloseOutlined, CloseSquareOutlined, ReloadOutlined} from "@ant-design/icons";
import moment from "moment";

export default function Home() {
    const [data, setData] = useState([])
    const [modal, setModal] = useState({
        isOpen: false,
        data: {},
    });
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [isModalCancelVisible, setIsModalCancelVisible] = useState(false);
    const [isModalReturnVisible, setIsModalReturnVisible] = useState(false);
    const [idCompleted, setIdCompleted] = useState();

    const showModal = () => {
        setIsModalVisible(true);

    };
    const handleOk = () => {
        setIsModalVisible(false);

        axios
            .put(`https://rookiesgroup3.azurewebsites.net/api/Assignments/${idCompleted}/accepted`)
            .then((res) => {


                window.location.reload();
                setIdCompleted(null)
            }).catch(() => {

        })


    };
    const handleCancel = () => {
        setIsModalVisible(false);
    };

    const handleCheckId = (id) => {
        setIdCompleted(id)
    }
//===============================================================
    const showModalDelete = () => {
        setIsModalCancelVisible(true);

    };
    const handleCheckDeleteId = (id) => {

        setIdCompleted(id)
    }
    const handleDeleteOk = () => {
        setIsModalCancelVisible(false);
        axios
            .put(`https://rookiesgroup3.azurewebsites.net/api/Assignments/${idCompleted}/declined`)
            .then((res) => {
                setIdCompleted(null)

                window.location.reload();
            }).catch((error) => {

        })
    }
    const handleCancelModal = () => {
        setIsModalCancelVisible(false);
    };
//===========================================================
    const showModalReturn = () => {
        setIsModalReturnVisible(true);
    }
    const handleCheckReturnId = (id) => {
        setIdCompleted(id)
    }
    const handleReturnOk = () => {
        setIsModalReturnVisible(false);
        axios
            .post(`${process.env.REACT_APP_UNSPLASH_REQUEST_FOR_RETURNING_URL}/user/${idCompleted}`)
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
//===============================================
    useEffect(() => {
        axios
            .get(`${process.env.REACT_APP_UNSPLASH_MY_ASSIGNMENT_URL}`, {})
            .then((response) => {
                let respData = response.data
                respData.forEach((element) => {
                    element.state = element.state === 'WaitingForAcceptance' ? 'Waiting For Acceptance' : element.state;
                    element.assignedDate = moment(new Date(element.assignedDate).toLocaleDateString("en-US")).format('DD/MM/YYYY');

                    element.action = [
                        <Button
                            className='buttonState'
                            disabled={element.state === 'Accepted' || element.isInProgress === false}
                            onClick={() => {
                                showModal()
                                handleCheckId(element.id)
                            }}
                        >
                            <CheckOutlined
                                style={{color: 'red'}}
                            />
                        </Button>,
                        <Button
                            className="buttonState"
                            disabled={element.state === 'Accepted' || element.isInProgress === false}
                            onClick={() => {
                                showModalDelete()
                                handleCheckDeleteId(element.id)
                            }}
                        >
                            <CloseOutlined/>
                        </Button>,
                        <Button
                            className='buttonState'
                            disabled={element.state === 'Waiting For Acceptance' || element.isInProgress === false}
                            onClick={() => {
                                showModalReturn()
                                handleCheckReturnId(element.id)
                            }}
                        >
                            <ReloadOutlined
                                style={{color: 'blue'}}
                            /></Button>

                    ]
                })
                setData(response.data);


            })
            .catch((error) => {

            })
    }, [])

    const columns = [


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
                visible={modal.isOpen}
                title='Detail Assignment Information'
                onCancel={()=>{setModal({...modal,isOpen:false})}}
                closeIcon={<CloseSquareOutlined style={{color: "red", fontSize: "20px"}}/>}
                footer={
                    null
                }
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
                        <td style={{fontSize: '18px', color: '#838688'}}>Specification</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.specification}</td>
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
                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>Note</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.note}</td>
                    </tr>
                </table>


            </Modal>
            <Modal
                closable={false}
                title="Are You Sure?" visible={isModalVisible} okText="Yes" cancelText="No" onOk={handleOk}
                onCancel={handleCancel}
                footer={[
                    <div style={{textAlign: "left"}}>
                        <Button key="Yes" onClick={handleOk} className="buttonSave">Accept</Button>
                        <Button key="No" onClick={handleCancel} className='buttonCancel'>Cancel</Button>
                    </div>
                ]}>
                <p>Do you want to accept this assignment?</p>
            </Modal>
            <Modal
                closable={false}
                title="Are You Sure?" visible={isModalCancelVisible} okText="Yes" cancelText="No" onOk={handleDeleteOk}
                onCancel={handleCancelModal}
                footer={[
                    <div style={{textAlign: "left"}}>
                        <Button key="Yes" onClick={handleDeleteOk} className="buttonSave">Decline</Button>
                        <Button key="No" onClick={handleCancelModal} className=' buttonCancel'>Cancel</Button>
                    </div>
                ]}>
                <p>Do you want to decline this assignment?</p>
            </Modal>
            <Modal
                closable={false}
                title="Are You Sure?" visible={isModalReturnVisible} okText="Yes" cancelText="No" onOk={handleReturnOk}
                onCancel={handleCancelReturnModal}
                footer={[
                    <div style={{textAlign: "left"}}>
                        <Button className="buttonSave" key="Yes" onClick={handleReturnOk}>Yes</Button>
                        <Button className="buttonCancel" key="No" onClick={handleCancelReturnModal}>No</Button>
                    </div>
                ]}>
                <p>Do you want to create a returning request for this asset?</p>
            </Modal>


            <div>
                <h1 style={{color: "red", float: "left"}}>My Assignment</h1>
                <Table
                    columns={columns}
                    dataSource={data}
                    onRow={(record) => {
                        return {
                            onClick: (e) => {


                                if (e.target.className !== 'ant-table-cell ant-table-cell-row-hover') {
                                    setModal({...modal, isOpen: false})
                                } else {

                                    setModal({
                                        ...modal, isOpen: true
                                        , data: {
                                            id: record.id,
                                            assetCode: record.assetCode,
                                            assetName: record.assetName,
                                            assignTo: record.assignTo,
                                            specification: record.specification,
                                            assignedBy: record.assignedBy,
                                            assignedDate: record.assignedDate,
                                            state: record.state,
                                            note: record.note
                                        }

                                    });

                                }


                            }
                        }
                    }}
                >

                </Table>
            </div>


        </>
    )
}