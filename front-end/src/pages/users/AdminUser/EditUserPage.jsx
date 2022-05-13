import {Row, Col, Form, Input, Select, Button, DatePicker, Radio} from "antd";
import React, {useState} from "react";
import moment from "moment-timezone";
import "../../../styles/Styles.css";
import {useNavigate, useParams} from "react-router-dom";
import axios from "axios";

import "antd/dist/antd.css";

export default function EditUserPage() {
    const [isLoading, setLoading] = useState(false);
    const navigate = useNavigate();
    const userId = useParams().id;
    const [form] = Form.useForm();
    const {Option} = Select;
    const formItemLayout = {
        labelCol: {
            span: 6,
        },
        wrapperCol: {
            span: 18,
            offset: 1,
        },
    };




    React.useEffect(() => {
        axios
            .get(`${process.env.REACT_APP_UNSPLASH_USERURL}/${userId}`)
            .then(function (response) {

                form.setFieldsValue({
                    Firstname: response.data.firstname,
                    Lastname: response.data.lastname,
                    DateOfBirth: moment.tz(response.data.doB, "Asia/Ho_Chi_Minh"),
                    Gender: response.data.gender,
                    JoinedDate: moment.tz(response.data.joinDate, "Asia/Ho_Chi_Minh"),
                    Type: response.data.type,
                });
            })
            .catch(() => {

            });
    }, [ form, userId]);





    const onFinish = (fieldsValue) => {
        const values = {
            ...fieldsValue,
            DateOfBirth: fieldsValue["DateOfBirth"].format("YYYY-MM-DD"),
            JoinedDate: fieldsValue["JoinedDate"].format("YYYY-MM-DD"),
        };
        axios
            .put(`${process.env.REACT_APP_UNSPLASH_USERURL}`, {
                id: parseInt(userId),
                firstname: values.Firstname,
                lastname: values.Lastname,
                joinDate: values.JoinedDate,
                type: values.Type,
                doB: values.DateOfBirth,
                gender: values.Gender,
            })
            .then(() => {
                sessionStorage.setItem('changeStatus', true);
                navigate("/user");
            });

    };
    return (
        <Row>
            <Col span={12} offset={6}>
                <div className="content">
                    <Row style={{marginBottom: "10px"}} className="fontHeaderContent">
                        Edit User
                    </Row>
                    <Row
                        style={{marginTop: "10px", marginLeft: "5px", display: "block"}}
                    >
                        {/* Form */}
                        <Form
                            form={form}
                            name="complex-form"
                            onFinish={onFinish}
                            {...formItemLayout}
                            labelAlign="left"

                        >
                            <Form.Item label="First Name" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="Firstname"
                                    rules={[{required: true}]}
                                    style={{display: "block"}}
                                >
                                    <Input className="inputForm" disabled/>
                                </Form.Item>
                            </Form.Item>

                            <Form.Item label="Last Name" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="Lastname"
                                    rules={[{required: true}]}
                                    style={{display: "block"}}
                                >
                                    <Input className="inputForm" disabled/>
                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="Date Of Birth" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="DateOfBirth"
                                    rules={[{required: true, message: 'Date of birth must be required'},
                                        () => ({
                                            validator(_, value) {
                                                if ((new Date().getFullYear() - value._d.getFullYear()) < 18) {
                                                    return Promise.reject("User must be greater than 18 years old")
                                                }
                                                return Promise.resolve();
                                            }
                                        })
                                    ]}
                                    style={{display: "block"}}
                                >
                                    <DatePicker
                                        disabled={isLoading.isLoading===true}
                                        style={{display: "block"}}
                                        format="DD-MM-YYYY"
                                        className="inputForm"
                                    />
                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="Gender" style={{marginBottom: 0}}>
                                <Form.Item name="Gender" rules={[{required: true}]}>
                                    <Radio.Group disabled={isLoading.isLoading===true}>
                                        <Radio value="Female">Female</Radio>
                                        <Radio value="Male">Male</Radio>
                                    </Radio.Group>
                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="Joined Date" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="JoinedDate"
                                    rules={[{required: true, message: 'Joined date must be require',},
                                        ({getFieldValue}) => ({
                                            validator(_, value) {
                                                if (value._d.getDay() === 0 || value._d.getDay() === 6) {

                                                    return Promise.reject(`Join date can't be Satuday or Sunday `);
                                                }
                                                else if(value - getFieldValue('DateOfBirth') <568080000000){

                                                    return Promise.reject("Only accept staff from 18 years old");
                                                }
                                                else {
                                                    return Promise.resolve()
                                                }
                                            }
                                        })
                                    ]}
                                    style={{display: "block", marginLeft: ""}}
                                >
                                    <DatePicker
                                        disabled={isLoading.isLoading===true}
                                        style={{display: "block"}}
                                        format="DD-MM-YYYY"
                                        className="inputForm"
                                    />
                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="Type">
                                <Form.Item
                                    name="Type"
                                    rules={[{required: true}]}
                                    style={{display: "block"}}
                                >
                                    <Select
                                        disabled={isLoading.isLoading===true}
                                        showSearch
                                        name="Type"
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

                            <Form.Item shouldUpdate>
                                {()=>(
                                <Row style={{float: "right"}}>

                                        <Button
                                            disabled={
                                                !form.isFieldsTouched(true) ||
                                                form.getFieldsError().filter(({errors}) => errors.length).length > 0
                                            }
                                            className ="buttonSave"

                                            loading={isLoading} onClick={() => {
                                            setLoading({isLoading:true})
                                            setTimeout(()=>{
                                                setLoading({isLoading : false})
                                            },3000)
                                            onFinish()
                                        }} htmlType="submit">
                                            Save
                                        </Button>
                                        <Button
                                            className="buttonCancel"
                                            disabled={isLoading.isLoading===true}
                                            onClick={() => {
                                            navigate("/user");
                                        }}>Cancel</Button>


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
