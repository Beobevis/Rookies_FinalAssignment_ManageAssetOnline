import React, {useState} from "react";
import {Modal, Input, Button, Form} from "antd";
import {EyeOutlined, EyeInvisibleOutlined} from "@ant-design/icons";
import axios from "axios";
import "antd/dist/antd.css";
import "../styles/Styles.css";

export default function ChangePasswordModal({isOpen, userid}) {
    const [isPaswordVisible, setIsPaswordVisible] = useState(false);
    const [isModalVisible, setIsModalVisible] = useState(isOpen);
    const [NewPassword, setNewPassword] = useState("");
    const [error, setError] = useState("");
    const [Loading, setLoading] = useState({
        isLoading: false
    })
    const handleOk = () => {
        axios
            .put(`${process.env.REACT_APP_UNSPLASH_CHANGEPASSWORD1ST_USERRURL}`, {
                id: userid,
                newPassword: NewPassword,
            })
            .then(function (response) {
                setIsModalVisible(false);

                localStorage.removeItem("loginState");
                window.location.href = "/login";
            })
            .catch((error) => {

                if (!error.response.data.title) {

                    setIsModalVisible(true);
                    setError(error.response.data.message);
                } else {
                    setIsModalVisible(true);
                    setError(error.response.data.title);
                }
            });
    };
    return (
        <>
            <Modal
                title="Change Password"
                visible={isModalVisible}
                maskClosable={false}
                onOk={handleOk}
                footer={[
                    <Button
                        className="buttonSave"
                         loading={Loading.isLoading} key="submit"
                        onClick={() => {
                            setLoading({...Loading, isLoading: true});
                            setTimeout(() => {
                                setLoading({...Loading, isLoading: false})
                            }, 3000)
                            handleOk();
                        }}>
                        Save
                    </Button>,
                ]}
                closable={false}
            >
                <p style={{fontFamily: "bold", fontSize: "16px"}}>
                    This is first time you logged in
                </p>
                <p style={{fontFamily: "bold", fontSize: "16px"}}>
                    You have changed your password to continue!.
                </p>
                <Form.Item label="New Password">
                    <Input

                        type={isPaswordVisible ? "text" : "password"}
                        onChange={(evt) => {
                            setNewPassword(evt.target.value);

                        }}
                        suffix={
                            isPaswordVisible ? (
                                <EyeOutlined
                                    onClick={() => {
                                        setIsPaswordVisible(!isPaswordVisible);
                                    }}
                                />
                            ) : (
                                <EyeInvisibleOutlined
                                    onClick={() => {
                                        setIsPaswordVisible(!isPaswordVisible);
                                    }}
                                />
                            )
                        }
                    />
                </Form.Item>
                <p style={{color: "red"}}>{error}</p>
            </Modal>
        </>
    );
}
