import {Row, Col, Form, Input, Select, Button, DatePicker, Radio} from "antd";
import React, {useState} from "react";
import "antd/dist/antd.css";
import "../../../styles/Styles.css";
import axios from "axios";
import {useNavigate} from "react-router-dom";

export default function CreateUserPage() {
    const [isLoading, setLoading] = useState({isLoading: false});
    const navigate = useNavigate();
    const {Option} = Select;
    const [form] = Form.useForm();
    const formItemLayout = {
        labelCol: {
            span: 6,
        },
        wrapperCol: {
            span: 18,
            offset: 1,
        },
    };

    const onFinish = (fieldsValue) => {
        const values = {
            ...fieldsValue,
            DateOfBirth: fieldsValue["DateOfBirth"].format("YYYY-MM-DD"),
            JoinedDate: fieldsValue["JoinedDate"].format("YYYY-MM-DD"),
        };

        axios
            .post(`${process.env.REACT_APP_UNSPLASH_USERURL}`, {
                firstname: values.Firstname,
                lastname: values.Lastname,
                joinDate: values.JoinedDate,
                type: values.Type,
                doB: values.DateOfBirth,
                gender: values.Gender,
            })
            .then(() => {
                setTimeout(() => {
                    setLoading({isLoading: false});
                }, 3000)


                navigate("/user");

            })
            .catch(() => {


            });
    };


    return (
        <Row>
            <Col span={12} offset={6}>
                <div className="content">
                    <Row style={{marginBottom: "10px"}} className="fontHeaderContent">
                        Create New User
                    </Row>
                    <Row
                        style={{marginTop: "10px", marginLeft: "5px", display: "block"}}
                    >
                        <Form
                            form={form}
                            // initialValues={{Gender: 'Female'}}
                            name="complex-form"
                            onFinish={onFinish}
                            {...formItemLayout}
                            labelAlign="left"
                        >
                            <Form.Item label="First name" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="Firstname"
                                    rules={[{required: true, message: 'First name must be required'},
                                        {
                                            pattern: new RegExp("^[a-zA-Z'. ]+$"),
                                            message: 'First name can not have number'
                                        }
                                        , {max: 50, message: "First name must less than 50 characters"}
                                    ]}
                                    style={{display: "block"}}
                                    hasFeedback
                                >
                                    <Input disabled={isLoading.isLoading === true} maxLength={51}
                                           className="inputForm"/>
                                </Form.Item>
                            </Form.Item>

                            <Form.Item label="Last name" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="Lastname"
                                    rules={[{required: true, message: 'Last name must be required'},
                                        {
                                            pattern: new RegExp("^[a-zA-Z'. ]+$"),
                                            message: 'Last name can not have number'
                                        }
                                        , {max: 50, message: "Last name must less than 50 characters"}
                                    ]}
                                    style={{display: "block"}}
                                    hasFeedback
                                >
                                    <Input disabled={isLoading.isLoading === true} maxLength={51}
                                           className="inputForm"/>
                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="Date of Birth" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="DateOfBirth"
                                    rules={[{required: true, message: 'Date of birth must be required'},
                                        () => ({
                                            validator(_, value) {
                                                if ((new Date().getFullYear() - value._d.getFullYear()) < 18) {
                                                    return Promise.reject("User is under 18. Please select a different date")
                                                }
                                                return Promise.resolve();
                                            }
                                        })
                                    ]}
                                    style={{display: "block"}}
                                    hasFeedback
                                >
                                    <DatePicker
                                        disabled={isLoading.isLoading === true}
                                        style={{display: "block"}}
                                        format="DD-MM-YYYY"

                                        className="inputForm"
                                    />

                                </Form.Item>
                            </Form.Item>
                            <Form.Item disabled={isLoading.isLoading === true} label="Gender" style={{marginBottom: 0}}>
                                <Form.Item name="Gender" rules={[{required: true, message: 'Gender must be required'}]}>
                                    <Radio.Group disabled={isLoading.isLoading === true}>
                                        <Radio value="Female">Female</Radio>
                                        <Radio value="Male">Male</Radio>
                                    </Radio.Group>
                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="Joined Date" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="JoinedDate"
                                    rules={[{required: true, message: 'Joined date must be required',},
                                        ({getFieldValue}) => ({
                                            validator(_, value) {

                                                if (value === null || value === "") {
                                                    return Promise.resolve()
                                                }
                                                if (value._d.getDay() === 0 || value._d.getDay() === 6) {

                                                    return Promise.reject(`Joined date is Saturday or Sunday. Please select a different date `);

                                                } else if (value - getFieldValue('DateOfBirth') < 568080000000) {

                                                    return Promise.reject("Only accept staff from 18 years old");
                                                } else {
                                                    return Promise.resolve()
                                                }
                                            }
                                        })
                                    ]}
                                    style={{display: "block", marginLeft: ""}}
                                    hasFeedback
                                >

                                    <DatePicker
                                        disabled={isLoading.isLoading === true}
                                        style={{display: "block"}}
                                        format="DD-MM-YYYY"

                                        className="inputForm"

                                    />

                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="Type">
                                <Form.Item
                                    name="Type"
                                    rules={[{required: true, message: 'Type must be required'}]}
                                    style={{display: "block"}}
                                    hasFeedback
                                >
                                    <Select
                                        disabled={isLoading.isLoading === true}
                                        showSearch
                                        className="inputForm"
                                        style={{display: "block"}}
                                        optionFilterProp="children"
                                        filterOption={(input, option) =>
                                            option.children
                                                .toLowerCase()
                                                .indexOf(input.toLowerCase()) >= 0
                                        }
                                        filterSort={(optionA, optionB) =>
                                            optionA.children
                                                .toLowerCase()
                                                .localeCompare(optionB.children.toLowerCase())
                                        }
                                    >
                                        <Option value="Staff">Staff</Option>
                                        <Option value="Admin">Admin</Option>
                                    </Select>
                                </Form.Item>
                            </Form.Item>

                            <Form.Item shouldUpdate >
                                {() => (
                                <Row style={{float: 'right'}}>
                                        < Button
                                            disabled={
                                                !form.isFieldsTouched(true) ||
                                                form.getFieldsError().filter(({errors}) => errors.length).length > 0
                                            }
                                            className="buttonSave"
                                            style={{background: "#e30c18", color: "white"}}
                                            loading={isLoading.isLoading} htmlType="submit" onClick={() => {
                                            setLoading({isLoading: true})
                                            setTimeout(() => {
                                                    setLoading({isLoading: false})
                                                }, 2000
                                            )
                                            onFinish()
                                        }}
                                        >
                                            Save
                                        </Button>
                                    <Button
                                        className="buttonCancel"
                                        disabled={isLoading.isLoading === true} onClick={() => {
                                        navigate("/user");
                                    }}>
                                        Cancel
                                    </Button>

                                </Row>

                                )}
                            </Form.Item>
                        </Form>
                    </Row>
                </div>
            </Col>
        </Row>
    );
}
