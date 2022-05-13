import {
    Row,
    Col,
    Form,
    Input,
    Select,
    Button,
    DatePicker,
    Radio,
    Modal,
} from "antd";
import React, {useEffect, useState} from "react";
import {useNavigate} from "react-router-dom";
import axios from "axios";
import "antd/dist/antd.css";
import moment from "moment";
//   import "../styles/CreateAssetPage.css";

export default function CreateAssetPage() {
    const [isLoading, setLoading] = useState({isLoading: false});
    const navigate = useNavigate();
    const [cateData, setCateData] = useState([]);
    const [submitData, setSubmitData] = useState({
        assetName: "",
        categoryId: 1,
        specification: "",
        installedDate: "",
        state: "",
        categoryName: "",
        prefix: "",
    });

    const [newCategory, setNewCategory] = useState({
        categoryName: "",
        prefix: "",
    });
    const [error, setError] = useState("");

    const [form] = Form.useForm();

    const [popModal, setpopModal] = useState({
        isOpen: false,
        ispopLoading: false
    });
    const formItemLayout = {
        labelCol: {
            span: 7,
        },
        wrapperCol: {
            span: 15,
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
        fetchData(`${process.env.REACT_APP_UNSPLASH_CATEGORY}`, "GET").then(
            (res) => {
                setCateData(res.data);
            }
        );
        return () => {

        };
    }, []);
    const handleCateSelection = (cate) => {
        if (cate === "add") {
            setpopModal({...popModal, isOpen: true});
            form.setFieldsValue(
                {...submitData, categoryId: 1}
            )

        } else {
            setSubmitData({
                ...submitData,
                categoryId: parseInt(cate),
            });
        }


    };
    const handleNameChange = (name) => {

        setSubmitData({
            ...submitData,
            assetName: name.target.value,
        });
    };
    const handleSpecificationChange = (name) => {

        setSubmitData({
            ...submitData,
            specification: name.target.value,
        });
    };
    const handleInstallDateChange = (name, Datestring) => {

        setSubmitData({
            ...submitData,
            installedDate: Datestring,
        });
    };
    const handleStateChange = (name) => {

        setSubmitData({
            ...submitData,
            state: name.target.value,
        });
    };
    const handleCreate = (fieldsValue) => {


        const values = {
            ...fieldsValue,
            installedDate: fieldsValue["installedDate"].format("YYYY-MM-DD"),

        };
        fetchData(`${process.env.REACT_APP_UNSPLASH_ASSETURL}`, "POST", {
            ...submitData,
            installedDate: values.installedDate
        })
            .then(() => {
                setSubmitData({
                    assetName: "",
                    categoryId: "",
                    specification: "",
                    installedDate: "",
                    state: "",
                });
                navigate("/asset");
            })
            .catch(() => {

            });
    };

    const handleOk = () => {
        fetchData(`${process.env.REACT_APP_UNSPLASH_CATEGORY}`, "POST", newCategory)
            .then((res) => {

                // setpopModal(false);
                window.location.reload();
            })
            .catch((error) => {

                if (!error.response.data.title) {

                    setpopModal({...popModal, isOpen: true});
                    setError(error.response.data.message);
                } else {
                    setpopModal({...popModal, isOpen: true});
                    !error.response.data.errors.CategoryName
                        ? setError(error.response.data.errors.Prefix)
                        : setError(error.response.data.errors.CategoryName);
                }
            });
    };


    return (
        <Row>
            <Modal
                visible={popModal.isOpen}
                onCancel={() => {
                    setNewCategory({categoryName: "", prefix: ""});
                    setpopModal(false);
                    setError("");
                    navigate("/createAsset");

                }}
                onOk={() => {
                    handleOk();
                }}
                footer={[
                    <Button
                        style={{background: "#e30c18", color: "white"}}
                        loading={popModal.ispopLoading} key="ok" onClick={() => {
                        setpopModal({...popModal, ispopLoading: true});
                        setTimeout(() => {
                            setpopModal({...popModal, ispopLoading: false})
                        }, 5000)
                        handleOk();
                    }}>Save</Button>,
                    <Button disabled={popModal.ispopLoading === true} key="back" onClick={() => {
                        setNewCategory({categoryName: "", prefix: ""});
                        setpopModal({...popModal, isOpen: false});
                        setError("");
                        navigate("/createAsset");
                    }}>Cancel</Button>
                ]}

                destroyOnClose={true}
                maskClosable={false}
                closable={false}
            >
                <Form {...formItemLayout}>
                    <Form.Item
                        style={{marginTop: "20px"}}
                        name='CategoryName'
                        label="Category name"
                        rules={[{required: true, message: 'Category name must be required'},
                            {whitespace: true, message: 'Category name can not be empty'}, {
                                max: 50,
                                message: 'Category name must be less than 50 characters'
                            }
                        ]}
                    >
                        <Input
                            disabled={popModal.isLoading === true}
                            onChange={(name) => {

                                setNewCategory({
                                    ...newCategory,
                                    categoryName: name.target.value,
                                });

                            }}
                        />
                    </Form.Item>
                    <Form.Item label="Prefix" name="prefix"
                               rules={[{required: true, message: 'Prefix must be required'},
                                   {max: 5, message: 'Prefix must be less than 5 characters'}
                               ]}>
                        <Input
                            disabled={popModal.isLoading === true}
                            onChange={(pre) => {
                                setNewCategory({...newCategory, prefix: pre.target.value});
                            }}
                        />
                    </Form.Item>
                </Form>
                <p style={{color: "red"}}>{error}</p>
            </Modal>

            <Col span={12} offset={4}>
                <div className="content">
                    <Row style={{marginBottom: "10px"}} className="fontHeaderContent">
                        Create New Asset
                    </Row>
                    <Row
                        style={{marginTop: "10px", marginLeft: "5px", display: "block"}}
                    >
                        <Form
                            name="complex-form"
                            // initialValues={{State: 'Available'}}
                            {...formItemLayout}
                            onFinish={handleCreate}
                            labelAlign="left"
                            form={form}
                        >

                            <Form.Item label="Name" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="assetName"
                                    rules={[{required: true, message: "Asset name must be required"},
                                        {whitespace: true, message: 'Asset can not be empty'},
                                        {max: 50, message: 'Asset must be less than 50 characters '}
                                    ]}
                                    style={{display: "block"}}
                                    hasFeedback
                                >

                                    <Input
                                        disabled={isLoading.isLoading === true}
                                        className="inputForm"
                                        value={submitData.assetName}
                                        onChange={handleNameChange}
                                        maxLength={51}
                                    />
                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="Category">
                                <Form.Item
                                    name="categoryId"
                                    rules={[{required: true, message: 'Category must be required'}]}
                                    style={{display: "block"}}
                                    hasFeedback
                                >
                                    <Select
                                        disabled={isLoading.isLoading === true}
                                        style={{display: "block"}}
                                        className="inputForm"
                                        id="select"

                                        onSelect={handleCateSelection}
                                    >
                                        {cateData.map(function (category) {
                                            return (
                                                <Select.Option


                                                    key={category.id}
                                                    value={category.id}

                                                    // onClick={handleCateSelection}
                                                >

                                                    {category.categoryName}
                                                </Select.Option>
                                            );
                                        })}
                                        <Select.Option style={{color: "blue"}} value="add">
                                            Add new category
                                        </Select.Option>
                                    </Select>
                                </Form.Item>
                            </Form.Item>

                            <Form.Item label="Specifications" style={{marginBottom: 40}}>
                                <Form.Item
                                    name="specification"
                                    rules={[{required: true, message: "Specification must be required"},
                                        {whitespace: true, message: 'Specification can not be empty'},
                                        {max: 500, message: 'Specification must be less than 500 characters.'}
                                    ]}
                                    hasFeedback
                                >
                                    <Input.TextArea
                                        disabled={isLoading.isLoading === true}
                                        className="inputForm"
                                        value={submitData.specification}
                                        onChange={handleSpecificationChange}
                                    />
                                </Form.Item>
                            </Form.Item>
                            <Form.Item label="Installed Date" style={{marginBottom: 0}}>
                                <Form.Item
                                    name="installedDate"
                                    rules={[{required: true},
                                        () => ({
                                            validator(_, value) {
                                                if (moment(new Date()).isBefore(value._d)) {
                                                    return Promise.reject(' Installed date should not be in the future date')
                                                } else {
                                                    return Promise.resolve()
                                                }

                                            }
                                        })
                                    ]}
                                    style={{display: "block"}}
                                    hasFeedback
                                >
                                    <DatePicker
                                        disabled={isLoading.isLoading === true}
                                        style={{display: "block"}}
                                        className="inputForm"
                                        format="DD-MM-YYYY"
                                        value={submitData.installedDate}
                                        onChange={handleInstallDateChange}

                                    />
                                </Form.Item>
                            </Form.Item>
                            <Form.Item  label="State" style={{marginBottom: 0}}>
                                <Form.Item name="State"
                                           rules={[{required: true, message: "State must be required"}]}
                                           hasFeedback
                                >
                                    <Radio.Group disabled={isLoading.isLoading === true} onChange={handleStateChange}>
                                        <Radio value="Available" name="state">
                                            Available
                                        </Radio>
                                        <Radio value="NotAvailable" name="state">
                                            Not Available
                                        </Radio>
                                    </Radio.Group>
                                </Form.Item>
                            </Form.Item>

                            <Form.Item shouldUpdate>
                                {() => (
                                    <Row style={{float: 'right'}}>
                                        <Button
                                            className="buttonSave"
                                            disabled={
                                                !form.isFieldsTouched(true) ||
                                                form.getFieldsError().filter(({errors}) => errors.length).length > 0
                                            }
                                            style={{width: "50px"}}
                                            loading={isLoading.isLoading}
                                            htmlType="submit"
                                            onClick={() => {
                                                setLoading({isLoading: true});
                                                setTimeout(() => {
                                                    setLoading({isLoading: false})
                                                }, 3000)
                                                handleCreate()
                                            }}
                                        >
                                            Save
                                        </Button>
                                        <Button
                                            className="buttonCancel"
                                            disabled={isLoading.isLoading === true}
                                            onClick={() => {
                                                navigate("/asset");
                                            }}
                                        >
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
