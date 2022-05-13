import React, {useState, useContext} from "react";
import {makeStyles} from "@material-ui/core/styles";
import {useFormik} from "formik";
import * as Yup from "yup";
import axios from "axios";
import {Context} from "../App";
import {Input, Spin, Button} from "antd";
import {LoadingOutlined} from "@ant-design/icons";
import {EyeInvisibleOutlined, EyeOutlined} from "@ant-design/icons";
import "antd/dist/antd.css";
import "../../src/styles/Styles.css";

const styles = makeStyles({
    form: {
        backgroundColor: "#FFF",
        padding: "50px",
        borderRadius: "10px",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        boxShadow: "-10px 10px 10px -5px rgba(0,0,0,0.75)",
        color: "white",
        margin: "auto",
        border: "3px solid red",
    },
    formContainer: {
        padding: "50px",
        display: "inline-block",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
    },
    formInput: {
        width: "100%",
        margin: "10px",
        height: "40px",
        borderRadius: "5px",
        border: "1px solid gray",
        padding: "5px",
        fontFamily: "'Roboto', sans-serif",
        color: "black",
        backgroundColor: "white",
    },
    formSubmit: {
        width: "50%",
        padding: "10px",
        borderRadius: "5px",
        color: "white !important",
        fontWeight: "bold",
        fontsize: "30px",
        background: "red",
        border: "2px solid red",
        fontFamily: "'Roboto', sans-serif",
        cursor: "pointer",

    },
    formMarketing: {
        display: "flex",
        margin: "20px",
        alignItems: "center",
    },
    validationText: {
        margin: "0px",
        fontSize: "1em",
        color: "red",
    },
    validContainer: {
        height: "20px",
    },
});
const LOGING = {
    LOADING: 'loading',
    FAIL: 'fail',
    SUCCESS: 'success',
    NONE: 'none'
}
const Login = () => {


    const [loginState, setLoginState] = useContext(Context);
    const [isLoging, setLoging] = useState(LOGING.NONE);
    const [error, setError] = useState("");
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);
    const formik = useFormik({
        initialValues: {
            username: "",
            password: "",
        },
        validationSchema: Yup.object({
            username: Yup.string().required("Required !"),
            password: Yup.string()
                .min(8, "Must be at least 8 characters !")
                .required("Required !"),
        }),

        onSubmit: () => {
            setLoging(LOGING.LOADING)

            axios
                .post(`${process.env.REACT_APP_UNSPLASH_USERURL}/Authenticate`, {
                    username: formik.values.username,
                    password: formik.values.password,
                })
                .then((response) => {
                    setLoginState({
                        ...loginState,
                        token: response.data.token,
                        isLogin: true,
                        role: response.data.role,
                        username: response.data.username,
                        isfirstlogin: response.data.isFirstLogin,
                        id: response.data.id,
                    });
                    localStorage.setItem(
                        "loginState",
                        JSON.stringify({
                            token: response.data.token,
                            isLogin: true,
                            role: response.data.role,
                            username: response.data.username,
                            isfirstlogin: response.data.isFirstLogin,
                            id: response.data.id,
                        })
                    );

                    axios.defaults.headers.common["Authorization"] =
                        "Bearer " + response.data.token;
                })

                .catch((error) => {
                    setLoging(LOGING.FAIL);
                    axios.defaults.headers.common["Authorization"] = "";
                    setError(error.response.data.message);
                    console.log(error.response.data);
                });
        },
    });

    const classes = styles();
    const antIcon = <LoadingOutlined style={{fontSize: "24px"}} spin/>;
    return (
        <>
            <div className={classes.formContainer}>
                <form className={classes.form} onSubmit={formik.handleSubmit}>
                    <img
                        alt="logo"
                        disabled
                        width={100}
                        src="https://assets-global.website-files.com/5da4969031ca1b26ebe008f7/602e42d8ec61635cd4859b25_Nash_Tech_Primary_Pos_sRGB.png"
                    />
                    <p style={{color: "red", fontWeight: "bold"}}>{error}</p>
                    <Input
                        type="username"
                        placeholder="Username"
                        className={classes.formInput}
                        name="username"
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        value={formik.values.username}
                    />

                    <div className={classes.validContainer}>
                        {formik.touched.username && formik.errors.username ? (
                            <p className={classes.validationText}>{formik.errors.username}</p>
                        ) : null}
                    </div>

                    <Input
                        type={isPasswordVisible ? "text" : "password"}
                        placeholder="Password"
                        className={classes.formInput}
                        name="password"
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        value={formik.values.password}
                        suffix={
                            isPasswordVisible ? (
                                <EyeOutlined
                                    onClick={() => {
                                        setIsPasswordVisible(!isPasswordVisible);
                                    }}
                                />
                            ) : (
                                <EyeInvisibleOutlined
                                    onClick={() => {
                                        setIsPasswordVisible(!isPasswordVisible);
                                    }}
                                />
                            )
                        }
                    />

                    <div className={classes.validContainer}>
                        {formik.touched.password && formik.errors.password ? (
                            <p className={classes.validationText}>{formik.errors.password}</p>
                        ) : null}
                    </div>

                    <Button  disabled={isLoging === LOGING.LOADING} htmlType="submit" style={{width: "100px", height: "40px",  background: "#e30c18", color: "white"}}>
                        <span>
                                {isLoging === LOGING.LOADING ? <Spin indicator={antIcon}/> :
                                    isLoging === LOGING.FAIL ? <div className="text-danger">Login Fail!</div> :
                                        "Login"}
                            </span>

                    </Button>
                </form>
            </div>
        </>
    );
};

export default Login;
