import React, {useState, useEffect} from "react";
import {Table, Input, Button, Menu, Dropdown, Row, Col, Modal} from "antd";
import {
    FilterOutlined,
    EditFilled,
    CloseCircleOutlined,
    LoadingOutlined,
    CloseSquareOutlined
} from "@ant-design/icons";
import {Link} from "react-router-dom";
import axios from "axios";
import "../../../styles/Styles.css";
import moment from "moment";
import "antd/dist/antd.css";

export default function ManageUser() {

    const [data, setData] = useState([]);
    const [searchText, setSearchText] = useState("");
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [type, setType] = useState("Type");
    
    const [modal, setModal] = useState({
        isOpen: false,
        data: {},

    });

    const columns = [

        {
            title: "Staff code",
            dataIndex: "staffCode",
            key: "staffCode",

            sorter: (a, b) => {
                if (a.staffCode > b.staffCode) {
                    return -1;
                }
                if (b.staffCode > a.staffCode) {
                    return 1;
                }
                return 0;
            },
        },
        {
            title: "Full Name",
            dataIndex: "fullName",
            key: "fullName",
            sorter: (a, b) => {
                if (a.fullName > b.fullName) {
                    return -1;
                }
                if (b.fullName > a.fullName) {
                    return 1;
                }
                return 0;
            },
        },
        {
            title: "Username",
            dataIndex: "username",
            key: "username",
        },
        {
            title: "Joined Date",
            dataIndex: "joinDateFomat",
            key: "joinDateFomat",
            sorter: (a, b) => {
                if (a.joinDate > b.joinDate) {
                    return -1;
                }
                if (b.joinDate > a.joinDate) {
                    return 1;
                }
                return 0;
            },
        },
        {
            title: "Type",
            dataIndex: "type",
            key: "type",
            sorter: (a, b) => {
                if (a.type > b.type) {
                    return -1;
                }
                if (b.type > a.type) {
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
    const [deleteModal, setDeleteModal] = useState({
        isOpen: false,
        title: "Are you sure?",
        content: (<p>Do you want to disable user?</p>),
        footer: (<div style={{ textAlign: 'left' }} >
            <Button className="buttonSave" >Disable</Button>
            <Button className="buttonCancel">Cancel</Button>
        </div>)
    })
    useEffect(() => {
        axios
            .get(`${process.env.REACT_APP_UNSPLASH_USERURL}`, {})
            .then(function (response) {


                let respData = response.data;

                respData.forEach((element) => {
                    element.fullName = element.firstname + " " + element.lastname;
                    element.joinDateFomat = moment(new Date(element.joinDate).toLocaleDateString("en-US")).format('DD/MM/YYYY');
                    element.action = [
                        <Link to={`/editUser/${element.id}`} id="editButton">
                            <EditFilled style={{ color : 'green',fontSize: "13px"}}/>
                        </Link>,
                        <CloseCircleOutlined

                            onClick={() => {
                                setDeleteModal({
                                    ...deleteModal,
                                    footer: (<div style={{ textAlign: 'left' }} >
                                        <Button
                                            className="buttonSave"
                                            onClick={() => {
                                                axios.delete(`https://rookiesgroup3.azurewebsites.net/api/Users?id=${element.id}`).then(
                                                    () => {

                                                        setDeleteModal({ ...deleteModal, isOpen: false })
                                                        window.location.reload();
                                                    }).catch(() => {

                                                        setDeleteModal({
                                                            ...deleteModal, isOpen: true
                                                            , footer: null
                                                            , title: 'Can not disable user'
                                                            , content: (<p>
                                                                There are valid assignments belonging to this user. Please Close all assignments before disabling user.
                                                            </p>),
    
                                                        })
                                                    })
                                            }}
                                        >Disable</Button>
                                        <Button className="buttonCancel" onClick = {() =>{
                                            setDeleteModal({...deleteModal, isOpen: false })
                                        }}>Cancel</Button>
                                    </div>)
                                    , isOpen: true
                                })
                            }}
                            style={{ color : 'red',fontSize: "13px",marginLeft:"10px"}}
                        />,
                    ];
                });
                setData(respData.sort((a, b) => {
                    if (a.staffCode.trim().toLowerCase() > b.staffCode.trim().toLowerCase()) {
                        return 1;
                    }
                    if (b.staffCode.trim().toLowerCase() > a.staffCode.trim().toLowerCase()) {
                        return -1;
                    }
                    return 0;
                }));
            }, [])
            .catch(() => {

            });
    }, [deleteModal]);

    
    const dataBytype =
        type === "Type"
            ? data
            : data.filter((u) => u.type === type);
    const finalData =
        searchText === ""
            ? dataBytype
            : dataBytype.filter(
                (u) =>
                    (u.fullName.toLowerCase()).replace(/\s+/g, '').includes(searchText.toLowerCase().replace(/\s+/g, '')) ||
                    u.staffCode.toLowerCase().includes(searchText.toLowerCase())
            );

    const pagination = {
        current: page,
        PageSize: pageSize,
        total: finalData.length,
        pageSizeOptions: [5, 10, 15, 20],
        className: "ant-btn-dangerous",
        dangerous: true,
        onChange: (page, pageSize) => {
            setPage(page);
            setPageSize(pageSize);
        },
    };


    return (
        <>
            <p style={{ display :'block' ,fontSize:"20px", margin : '0 auto', textAlign:'left', color : ' red', fontWeight : 'bold', paddingBottom:"20px"}}>User List</p>
            <Row gutter={45} style={{marginBottom: "30px"}}>

                <Col xs={8} sm={8} md={7} lg={7} xl={6} xxl={5}>
                    <Dropdown.Button
                        placement="bottom"
                        icon={<FilterOutlined/>}
                        overlay={
                            <Menu>
                                <Menu.Item
                                    value="staff"
                                    onClick={() => {
                                        setType("Staff");
                                    }}
                                >
                                    Staff
                                </Menu.Item>
                                <Menu.Item
                                    value="Admin"
                                    onClick={() => {
                                        setType("Admin");
                                    }}
                                >
                                    {" "}
                                    Admin
                                </Menu.Item>
                                <Menu.Item
                                    onClick={() => {
                                        setType("Type");
                                    }}
                                >
                                    All
                                </Menu.Item>
                            </Menu>
                        }
                    >
                        {type}
                    </Dropdown.Button>
                </Col>
                <Col xs={8} sm={8} md={7} lg={7} xl={8} xxl={8}>
                    <Input.Search
                        placeholder="Search User"
                        maxLength={255}
                        allowClear
                        onSearch={(e) => {
                            setPage(1)
                            setSearchText(e.replace(/ /g, ''))
                        }}


                    />
                </Col>
                <Col xs={8} sm={8} md={7} lg={7} xl={9} xxl={9}>
                    <Button style={ {background: "#e30c18", color: "white"}}>
                        <Link to="/createUser"> Create new user</Link>
                    </Button>
                </Col>
            </Row>

    {/* Delete Modal */}
    <Modal
                visible={deleteModal.isOpen}
                title={deleteModal.title}
                footer={deleteModal.footer}
                onCancel={() => { setDeleteModal({ ...deleteModal, isOpen: false }) }}
                destroyOnClose={true}
                closeIcon={<CloseSquareOutlined style={{ color: "red", fontSize: "20px" }} />}
            >
                {deleteModal.content}
            </Modal>

           
            <Modal
                visible={modal.isOpen}
                title='Detail User'
                onOk={() => {
                    setModal({...modal, isOpen: false});
                }}
                footer={[
                    <Button
                    style={ {background: "#e30c18", color: "white"}}
                    key="back" onClick={() => {
                        setModal({...modal, isOpen: false});
                    }
                    }>Close</Button>
                ]}
                closable={false}

            >
            

                <table
                >
                    <tr>

                        <td style={{fontSize: '18px', color: '#838688'}}>ID</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.id}</td>
                    </tr>
                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>Staff Code</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.staffCode}</td>
                    </tr>
                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>Full Name</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.fullName}</td>
                    </tr>
                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>Location</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.location}</td>
                    </tr>
                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>Date of birth</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.dob}</td>
                    </tr>
                    <tr>
                        <td style={{fontSize: '18px', color: '#838688'}}>Gender</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.gender}</td>
                    </tr>
                </table>


            </Modal>

            { data.length === 0 ? <Table
                loading={{indicator: <LoadingOutlined style={{fontSize:"60px", color:"red"}}  />, spinning: true}}/> :
            <Table

                key="id"
                rowKey={(data) => data.id}
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
                                        id: record.id,
                                        staffCode: record.staffCode,
                                        fullName: record.fullName,
                                        location: record.location,
                                        dob: moment(new Date(record.doB).toLocaleDateString('en-US')).format(' DD/MM/YYYY'),
                                        gender: record.gender
                                    }
                                });
                            }else if(e.target.className === 'ant-table-cell ant-table-column-sort ant-table-cell-row-hover'){
                                setModal({
                                    ...modal, isOpen: true
                                    , data: {
                                        id: record.id,
                                        staffCode: record.staffCode,
                                        fullName: record.fullName,
                                        location: record.location,
                                        dob: moment(new Date(record.doB).toLocaleDateString('en-US')).format(' DD/MM/YYYY'),
                                        gender: record.gender
                                    }
                                });
                            }
                             
                            else {
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
