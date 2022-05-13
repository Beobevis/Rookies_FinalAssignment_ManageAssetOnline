import React from "react";
import "antd/dist/antd.css";
import { DownOutlined, LogoutOutlined, UserOutlined } from "@ant-design/icons";
import { Menu, Dropdown, Modal, Form, Input, Button } from "antd";
import { PageHeader } from "antd";
import axios from "axios";
import "antd/dist/antd.css";
import { useLocation } from "react-router-dom";
import { AppRoutes } from "../routes/AppRoutes";
import "../styles/Styles.css";

export default function HeaderComponent({ username }) {
    
    const [isModal, setModal] = React.useState({
        isOpen: false,
        isLoading: false,
    });
    const [password, setPassword] = React.useState({
        oldPassword: "",
        newPassword: "",
    });
    const [changeSuccess, setChangeSuccess] = React.useState(false);
    const [Footer, setFooter] = React.useState({});
    const [error, setError] = React.useState("");
    const [navTitle,setNavTitle] = React.useState('Home');
    let {pathname} = useLocation()
     
    

    React.useEffect(() => {
        AppRoutes.forEach(
            route=>{
                 if(route.path === pathname.replace(/\d+/g,':id')){
                     setNavTitle(route.title)
                 }
            }
        )
    })

    const formItemLayout = {
        labelCol: {
            span: 6,
        },
        wrapperCol: {
            span: 16,
            offset: 1,
        },
    };

    const handleConfirmLogout = () => {
        Modal.confirm({
           
            title: "Are you sure?",
            icon: <LogoutOutlined style={{ color: 'red' }} />,
            content: "Do you want to log out?",
            okText: "Logout",
            cancelText: "Cancel",
            okButtonProps:{style:{ background: "#e30c18", color: "white"}},
            
            onOk() {
                return new Promise((resolve, reject) => {
                    setTimeout(Math.random() > 0.5 ? resolve : reject, 5000);
                    localStorage.removeItem("loginState");
                    axios.get(`${process.env.REACT_APP_UNSPLASH_LOGOUT}`);
                    window.location.href = `${process.env.REACT_APP_UNSPLASH_BASEFEURL}`;
                })
            },
            onCancel() {
            },
            
        });
    };

    const dropdownuser = (
        <Menu>



            <Menu.Item
                key="change password"
                onClick={() => {
                    setModal({ ...isModal, isOpen: true });
                }}
            >
                <UserOutlined style={{color: 'red', fontWeight:"bold"}}/> Change Password
            </Menu.Item>
            <Menu.Divider />
            <Menu.Item onClick={handleConfirmLogout} key="3">
               <LogoutOutlined style={{color: 'red', fontWeight: "bold"}} /> Logout
            </Menu.Item>
        </Menu>
    );

    return (
        <>
        
            <Modal
                afterClose={() => {
                    setError("");
                }}
                closable={false}
                cancelText='Cancel'
                okText='Save'
               okButtonProps={{style:{ background: "#e30c18", color: "white"}}}

                visible={isModal.isOpen}
                footer={[
                    <Button
                        className = "buttonSave"
                        loading={isModal.isLoading} key="save" onClick={() => {
                        setModal({ ...isModal, isLoading: true })
                        setTimeout(() => {
                            setModal({ ...isModal, isLoading: false })
                        }, 3000)
                        axios
                            .put(`${process.env.REACT_APP_UNSPLASH_CHANGEPW_USER}`, password)
                            .then(() => {

                                setChangeSuccess(true);

                                setFooter({
                                    footer: (
                                        <Button
                                            className = "buttonSave"
                                            onClick={() => {
                                                setFooter({});
                                                setChangeSuccess(false);
                                                setModal({ ...isModal, isOpen: false });
                                            }}
                                        >
                                            Close
                                        </Button>
                                    ),
                                });
                            })
                            .catch((error) => {

                                if (!error.response.data.title) {

                                    setModal({ ...isModal, isOpen: true });
                                    setError(error.response.data.message);
                                } else {
                                    setModal({ ...isModal, isOpen: true });
                                    setError(error.response.data.title);
                                }
                            })
                    }
                    }>Save</Button>,
                    <Button
                        className = "buttonCancel"
                        disabled={isModal.isLoading === true} key="cancel" onClick={() => {
                        setModal({ ...isModal, isOpen: false });
                        setError("");
                    }
                    }>Cancel</Button>
                ]}
                onOk={() => {
                    axios
                        .put(`${process.env.REACT_APP_UNSPLASH_CHANGEPW_USER}`, password)
                        .then((response) => {

                            setChangeSuccess(true);

                            setFooter({
                                footer: (
                                    <Button
                                        className = "buttonCancel"
                                        onClick={() => {
                                            setFooter({});
                                            setChangeSuccess(false);
                                            setModal(false);
                                        }}
                                    >
                                        Close
                                    </Button>
                                ),
                            });
                        })
                        .catch((error) => {

                            if (!error.response.data.title) {

                                setModal(true);
                                setError(error.response.data.message);
                            } else {
                                setModal(true);
                                setError(error.response.data.title);
                            }
                        });
                }}
                onCancel={() => {
                    setModal(false);
                    setError("");
                }}
                destroyOnClose={true}
                title="Change Password"
                {...Footer}
            >
                {changeSuccess === false ? (
                    <Form {...formItemLayout}>
                        <Form.Item
                            name="Old Password"
                            style={{ marginTop: "20px" }}
                            label="Old Password"
                            rules={[{ required: true, max: 50, }
                                , { pattern: new RegExp("^[a-zA-Z0-9]+$"), message: `Password can't have white space or special characters` }
                            ]}
                            hasFeedback

                        >
                            <Input.Password
                                disabled={isModal.isLoading === true}
                                className="inputForm"
                                onChange={(old) => {
                                    setPassword({ ...password, oldPassword: old.target.value });
                                }}
                            />
                        </Form.Item>
                        <Form.Item
                            name="New Password"
                            label="New Password"
                            rules={[{ required: true, max: 50, whitespace: true },
                            { pattern: new RegExp("^[a-zA-Z0-9]+$"), message: `New password can't have white space or special characters` }
                            ]}
                            hasFeedback

                        >
                            <Input.Password
                                disabled={isModal.isLoading === true}
                                className="inputForm"
                                onChange={(newPass) => {
                                    setPassword({
                                        ...password,
                                        newPassword: newPass.target.value,

                                    });
                                }}
                            />
                        </Form.Item>
                        <p style={{ color: "red" }}>{error}</p>
                    </Form>
                ) : (
                    <p>Your password has been changed successfully!</p>
                )}


            </Modal>
            <PageHeader
                className="site-page-header"
                
            >
                <span style={{ color:'white',float: 'left',marginLeft:'20px', fontSize: "16px", fontWeight:"bold" }} >{navTitle}</span>
                <Dropdown  overlay={dropdownuser} trigger={["click"]}>
                    <a
                        style={{
                            float: "right",
                            margin: "auto",
                            fontSize: "20px",
                            color: "white",
                            marginRight:'30px'
                        }}
                        onClick={(e) => e.preventDefault()}
                        href="/#"
                    >
                        {username} <DownOutlined />
                    </a>
                </Dropdown>
            </PageHeader>
        </>
    );
}
