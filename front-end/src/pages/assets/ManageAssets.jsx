import React, {useState, useEffect} from "react";
import {Table, Input, Button, Menu, Dropdown, Row, Col, Modal} from "antd";
import {
    FilterOutlined,
    EditFilled,
    CloseCircleOutlined,
    LoadingOutlined,
    CloseSquareOutlined,

} from "@ant-design/icons";
import {Link, useParams} from "react-router-dom";
import axios from "axios";
import "antd/dist/antd.css";
import moment from "moment";

export default function ManageAssets() {
    const [data, setData] = useState([]);
    const [searchText, setSearchText] = useState("");
    const [page, setPage] = useState(1);
    const [modal, setModal] = useState({
        isOpen: false,
        data: {},
    });
    const assetId = useParams().id;
    const [pageSize, setPageSize] = useState(10);
    const [type, setType] = useState("State");
    const [cate, setCate] = useState([]);
    const [filterCate, setFilterCate] = useState("Category")

    const [historicalData, setHistoricalData] = useState([{
        id: "",
        assignTo: "",
        assignedBy: "",
        assignedDate: "",
        note: "",
    }]);

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
            title: "Category",
            dataIndex: "categoryName",
            key: "categoryName",
            sorter: (a, b) => {
                if (a.categoryName > b.categoryName) {
                    return -1;
                }
                if (b.categoryName > a.categoryName) {
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
    const HistoryColumns = [
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
            title: "Note",
            dataIndex: "note",
            key: "note",
        }

    ]

    const [deleteModal, setDeleteModal] = useState({
        isOpen: false,
        title: "Are you sure?",
        content: (<p>Do you want to delete asset?</p>),
        footer: (<div style={{textAlign: 'left'}}>
            <Button>Delete</Button>
            <Button>Cancel</Button>
        </div>)
    })


    useEffect(() => {
        const getCategory = axios.get(`${process.env.REACT_APP_UNSPLASH_CATEGORY}`);
        const getAsset = axios.get(`${process.env.REACT_APP_UNSPLASH_ASSETURL}`)

        axios.all([getCategory, getAsset]).then(axios.spread((...responses) => {
            const categorys = responses[0].data;
            const assets = responses[1].data;
            setCate(categorys)
            assets.forEach(asset => {
                asset.state = asset.state === 'WaitingForRecycling' ? 'Waiting For Recycling' : asset.state === 'NotAvailable' ? 'Not Available' : asset.state;
                asset.categoryName = categorys.find(c => c.id === asset.categoryId).categoryName;
                asset.action = [
                    <Button
                        className="buttonState"
                        disabled={asset.state === 'Assigned'}>
                        <Link to={`/editAsset/${asset.id}`} id="editButton">
                            <EditFilled style={{color:"green",fontSize: "13px"}}/>
                        </Link>
                    </Button>
                    ,
                    <Button
                        className="buttonState"
                            disabled={asset.state === 'Assigned'}
                            onClick={() => {

                                setDeleteModal({
                                    ...deleteModal,
                                    footer: (<div style={{textAlign: 'left'}}>
                                        <Button
                                            className = ' buttonSave'
                                            onClick={() => {
                                                axios.delete(`https://rookiesgroup3.azurewebsites.net/api/Assets?id=${asset.id}`).then(
                                                    (response) => {

                                                        setDeleteModal({...deleteModal, is: false})
                                                        window.location.reload();
                                                    }).catch(() => {

                                                    setDeleteModal({
                                                        ...deleteModal, isOpen: true
                                                        , footer: null
                                                        , title: 'Can Not Delete Asset'
                                                        , content: (<p>
                                                            Cannot Delete Asset because it belongs to one
                                                            or more historical
                                                            assignments.<br/>
                                                            If the asset is not able to be used anymore,
                                                            please update its state in
                                                            <Link to={`/editAsset/${asset.id}`}> Edit asset
                                                                page</Link>

                                                        </p>),

                                                    })
                                                })
                                            }}
                                        >Delete</Button>
                                        <Button className = ' buttonCancel' onClick={() =>{
                                            setDeleteModal({...deleteModal, isOpen: false })
                                        }}>Cancel</Button>
                                    </div>)
                                    , isOpen: true
                                })
                            }}
                    >
                        <CloseCircleOutlined
                            id="deleteButton" className="Delete"
                            style={{color: "red", fontSize: "13px"}}

                        />
                    </Button>
                    ,
                ]
            })
            setData(assets.sort((a, b) => {
                if (a.assetCode.trim().toLowerCase() > b.assetCode.trim().toLowerCase()) {
                    return 1;
                }
                if (b.assetCode.trim().toLowerCase() > a.assetCode.trim().toLowerCase()) {
                    return -1;
                }
                return 0;
            }));
            // use/access the results 
        })).catch(errors => {

            // react on errors.
        })

    }, [deleteModal])


    const dataBytype = type === "State" ? data : data.filter((u) => u.state === type);
    const dataByCate = filterCate === "Category" ? dataBytype : dataBytype.filter((c) => c.categoryName === filterCate)
    const filterData =
        searchText === ""
            ? dataByCate
            : dataByCate.filter(
                (u) =>
                    (u.assetName.toLowerCase()).replace(/\s+/g, '').includes(searchText.toLowerCase().replace(/\s+/g, '')) ||
                    u.assetCode.toLowerCase().includes(searchText.toLowerCase())
            );

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

    return (
        <>
            <p style={{
                display: 'block',
                fontSize: "20px",
                margin: '0 auto',
                textAlign: 'left',
                color: ' red',
                fontWeight: 'bold',
                paddingBottom: "20px"
            }}>Asset List</p>
            <Row gutter={45} style={{marginBottom: "30px", display: "flex"}}>
                <Col xs={8} sm={8} md={6} lg={6} xl={5} xxl={5}>
                    <Dropdown.Button
                        placement="bottom"
                        icon={<FilterOutlined/>}
                        overlay={
                            <Menu>
                                <Menu.Item
                                    value="Available"
                                    onClick={() => {
                                        setType("Available");
                                    }}
                                >
                                    Available
                                </Menu.Item>
                                <Menu.Item
                                    value="Not Available"
                                    onClick={() => {
                                        setType("Not Available");
                                    }}
                                >

                                    Not available
                                </Menu.Item>
                                <Menu.Item
                                    value="Assigned"
                                    onClick={() => {
                                        setType("Assigned");
                                    }}
                                >
                                    Assigned
                                </Menu.Item>
                                <Menu.Item
                                    value="Waiting For Recycling"
                                    onClick={() => {
                                        setType("Waiting For Recycling");
                                    }}
                                >
                                    Waiting For Recycling
                                </Menu.Item>
                                <Menu.Item
                                    value="Recycled"
                                    onClick={() => {
                                        setType("Recycled");
                                    }}
                                >
                                    Recycled
                                </Menu.Item>
                                <Menu.Item

                                    onClick={() => {
                                        setType("State");
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
                <Col xs={8} sm={8} md={6} lg={6} xl={6} xxl={6}>
                    <Dropdown.Button
                        placement="bottom"
                        icon={<FilterOutlined/>}
                        overlay={
                            <Menu>
                                {cate.map(function (item) {
                                    return <Menu.Item
                                        key={item.id}
                                        value={item.id}
                                        onClick={() => {
                                            setFilterCate(item.categoryName)
                                        }}
                                    >
                                        {item.categoryName}
                                    </Menu.Item>
                                })}
                                <Menu.Item

                                    onClick={() => {
                                        setFilterCate("Category");
                                    }}
                                >
                                    All
                                </Menu.Item>

                            </Menu>
                        }
                    >
                        {filterCate}
                    </Dropdown.Button>
                </Col>
                <Col xs={5} sm={5} md={5} lg={3} xl={6} xxl={6}>
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
                <Col xs={8} sm={8} md={7} lg={7} xl={7} xxl={7}>
                    <Button style={{background: "#e30c18", color: "white"}}>
                        <Link to="/createAsset"> Create new Asset</Link>
                    </Button>
                </Col>

            </Row>
            <Row gutter={16} style={{marginBottom: '30px'}}>

            </Row>
            {/* Delete Modal */}
            <Modal
                visible={deleteModal.isOpen}
                title={deleteModal.title}
                footer={deleteModal.footer}
                closable={true}
                destroyOnClose={true}
                closeIcon={<CloseSquareOutlined style={{color: "red", fontSize: "20px"}}/>}
                onCancel={()=>{setDeleteModal({...deleteModal,isOpen:false})
            window.location.reload()
            }
            }
            >
                {deleteModal.content}
            </Modal>

            <Modal
                visible={modal.isOpen}
                title='Detail Asset'

                onCancel={() => {
                    setModal({...modal, isOpen: false});
                }}
                closeIcon={<CloseSquareOutlined style={{color: "red", fontSize: "20px"}}/>}
                footer={null}
                closable={true}
            >

                <table>
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
                        <td style={{fontSize: '18px', color: '#838688'}}>Category Name</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.categoryName}</td>
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
                        <td style={{fontSize: '18px', color: '#838688'}}>Installed Date</td>
                        <td style={{
                            fontSize: '18px',
                            color: '#838688',
                            textAlign: 'justify',
                            paddingLeft: '35px'
                        }}>{modal.data.installedDate}</td>
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
                </table>
                <div>
                    <Table
                        size="middle"
                        rowKey={assetId}
                        columns={HistoryColumns}
                        dataSource={historicalData}
                    />
                </div>


            </Modal>
            {data.length === 0 ? <Table
                    loading={{indicator: <LoadingOutlined style={{fontSize: "60px", color: "red"}}/>, spinning: true}}/> :
                <Table
                    rowKey="id"
                    columns={columns}
                    pagination={pagination}
                    dataSource={filterData}
                    onRow={(record) => {
                        return {
                            onClick: (e) => {

                                axios.get(`${process.env.REACT_APP_UNSPLASH_ASSETURL}/${record.id}`).then((response) => {
                                        const hisData = [];
                                        for (let i = 0; i < response.data.assignments.length; i++) {
                                            hisData.push({
                                                id: parseInt(response.data.assignments[i].id),
                                                assignTo: response.data.assignments[i].assignTo,
                                                assignedBy: response.data.assignments[i].assignedBy,
                                                assignedDate: moment(response.data.assignments[i].assignedDate).format(' DD/MM/YYYY'),
                                                note: response.data.assignments[i].note
                                            })
                                        }
                                        setHistoricalData(hisData)
                                    }
                                )
                                    .catch(error => {
                                        console.error(error.message);
                                    })

                                if (e.target.className === 'ant-table-cell ant-table-cell-row-hover') {
                                    setModal({
                                        ...modal, isOpen: true
                                        , data: {
                                            id: parseInt(record.id),
                                            assetCode: record.assetCode,
                                            assetName: record.assetName,
                                            categoryName: record.categoryName,
                                            installedDate: moment(new Date(record.installedDate).toLocaleDateString('en-US')).format(' DD/MM/YYYY'),
                                            state: record.state,
                                            specification: record.specification
                                        }
                                    });
                                } else if (e.target.className === 'ant-table-cell ant-table-column-sort ant-table-cell-row-hover') {

                                    setModal({
                                        ...modal, isOpen: true
                                        , data: {
                                            id: parseInt(record.id),
                                            assetCode: record.assetCode,
                                            assetName: record.assetName,
                                            categoryName: record.categoryName,
                                            installedDate: moment(new Date(record.installedDate).toLocaleDateString('en-US')).format(' DD/MM/YYYY'),
                                            state: record.state,
                                            specification: record.specification
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
