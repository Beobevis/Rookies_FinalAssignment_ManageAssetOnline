import {
    Row,
    Col,
    Form,
    Input,
    Select,
    Button,
    DatePicker,
    Radio,
    Space,
} from "antd";
import React, {useEffect,  useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import axios from "axios";
import "antd/dist/antd.css";

import moment from "moment-timezone";


export default function EditAssetPage() {
    const [Loading, setLoading] = useState({
        isLoading: false
    })
    const navigate = useNavigate();
    const [form] = Form.useForm();
    const assetId = useParams().id



    const formItemLayout = {
        labelCol: {
            span: 5,
        },
        wrapperCol: {
            span: 16,
            offset: 1,
        },
    };

    const fetchData = (url, method, data) => {
        return axios({
            method: method,
            url: url,
            data: data,
        });
    };

    useEffect(() => {
        const assetDetailRequest = fetchData(`${process.env.REACT_APP_UNSPLASH_ASSETURL}/${assetId}`, 'GET')
        const categorysRequest = fetchData(`${process.env.REACT_APP_UNSPLASH_CATEGORY}`, "GET")
        axios.all([assetDetailRequest, categorysRequest]).then(axios.spread((...response) => {

                const assetDetail = response[0].data
                const categorys = response[1].data

                form.setFieldsValue({
                    assetName: assetDetail.assetName,
                    specification: assetDetail.specification,
                    categoryId: assetDetail.categoryId,
                    installedDate: moment.tz(assetDetail.installedDate, "Asia/Ho_Chi_Minh"),
                    state: assetDetail.state,
                    categoryName: categorys.find(c => c.id === assetDetail.categoryId).categoryName,
                })

            })
        )
    }, [assetId, form])

    const onFinish = (fieldsValue) => {
        const values = {
            ...fieldsValue,
            installedDate: fieldsValue["installedDate"].format("YYYY-MM-DD")
        };
        fetchData(`${process.env.REACT_APP_UNSPLASH_ASSETURL}`, "PUT", {
            id: parseInt(assetId),
            assetName: values.assetName,
            categoryId: values.categoryId,
            specification: values.specification,
            installedDate: values.installedDate,
            state: values.state,
        })
            .then((res) => {

                navigate("/asset")

            })

    }


    return (
        <Row>

            <Col span={12} offset={4}>
                <div className="content">
                    <Row style={{marginBottom: "10px"}} className="fontHeaderContent">
                        Edit Asset
                    </Row>
                    <Row
                        style={{marginTop: "10px", marginLeft: "5px", display: "block"}}
                    >
                        <Form
                            form={form}
                            name="complex-form"
                            {...formItemLayout}
                            onFinish={onFinish}
                            labelAlign="left"


                        >
                            <Form.Item label="Name" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="assetName"
                                    rules={[{required: true, message: "Asset name must be required"},
                                        {whitespace: true, message: 'Asset can not be empty'},
                                        {max: 50, message: 'Asset must be less than 50 characters long'}]}
                                    style={{display: "block"}}
                                >
                                    <Input
                                        disabled={Loading.isLoading === true}
                                        className="inputForm"

                                    />
                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="Category">
                                <Form.Item

                                    name="categoryName"
                                    rules={[{required: true}]}
                                    style={{display: "block"}}
                                >
                                    <Select
                                        disabled
                                        style={{display: "block"}}
                                        className="inputForm"
                                        id="select"

                                    >


                                    </Select>
                                </Form.Item>
                            </Form.Item>

                            <Form.Item label="Specifications" style={{marginBottom: 40}}>
                                <Form.Item
                                    name="specification"
                                    rules={[{required: true, message: "Specification must be required"},
                                        {whitespace: true, message: 'Specification can not be empty'},
                                        {max: 500, message: 'Specification must be less than 500 characters long'}]}
                                    style={{display: "block"}}
                                >
                                    <Input.TextArea
                                        disabled={Loading.isLoading === true}
                                        className="inputForm"

                                    />
                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="Installed Date" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="installedDate"
                                    rules={[{required: true}]}
                                    style={{display: "block"}}
                                >
                                    <DatePicker
                                        disabled={Loading.isLoading === true}
                                        style={{display: "block"}}
                                        className="inputForm"
                                        format="DD-MM-YYYY"
                                    />
                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="State" style={{marginBottom: 0}}>
                                <Form.Item name="state" rules={[{required: true}]}>
                                    <Radio.Group disabled={Loading.isLoading === true}>
                                        <Space direction="vertical">
                                            <Radio value="Available" name="state">
                                                Available
                                            </Radio>
                                            <Radio value="NotAvailable" name="state">
                                                Not Available
                                            </Radio>
                                            <Radio value="WaitingForRecycling" name="state">
                                                Waiting for recycling
                                            </Radio>
                                            <Radio value="Recycled" name="state">
                                                Recycled
                                            </Radio>
                                        </Space>

                                    </Radio.Group>
                                </Form.Item>
                            </Form.Item>

                            <Form.Item shouldUpdate>
                                {()=>(
                                <div style={{float: 'right'}}>
                                    <Button
                                        className = "buttonSave"
                                        disabled={
                                            !form.isFieldsTouched(true) ||
                                            form.getFieldsError().filter(({errors}) => errors.length).length > 0
                                        }
                                        loading={Loading.isLoading === true}
                                        htmlType="submit"
                                        onClick={() => {
                                            setLoading({isLoading: true})
                                            setTimeout(() => {
                                                setLoading({isLoading: false})
                                            }, 3000)
                                            onFinish()
                                        }}
                                    >
                                        Save
                                    </Button>
                                    <Button
                                        className="buttonCancel"
                                        disabled={Loading.isLoading === true}
                                        onClick={() => {
                                            navigate("/asset");
                                        }}
                                    >
                                        Cancel
                                    </Button>
                                </div>
                                )}
                            </Form.Item>
                        </Form>
                    </Row>
                </div>
            </Col>
        </Row>
    );
}
