import {Row, Col} from "antd";

export default function GridComponent({leftComp, rightComp}) {
    return (
        <div>
            <Row gutter={10}>
                <Col xs={24} sm={24} md={6} lg={5} xl={5} xxl={4}>
                    <img
                        alt="logo"
                        style={{float: "left", display: "flex"}}
                        disabled
                        width={130}
                        src="https://assets-global.website-files.com/5da4969031ca1b26ebe008f7/602e42d8ec61635cd4859b25_Nash_Tech_Primary_Pos_sRGB.png"
                    />
                    <h2
                        style={{
                            float: "left",
                            color: "#cf2338",
                            display: "inline-flex",
                            padding: "5px",
                        }}
                    >
                        Online Asset Management
                    </h2>
                    {leftComp}
                </Col>
                <Col
                    style={{ padding: "30px" , paddingTop: "5px" }}
                    xs={24}
                    sm={24}
                    md={18}
                    lg={19}
                    xl={19}
                    xxl={20}
                >
                    {rightComp}
                </Col>
            </Row>
        </div>
    );
}
