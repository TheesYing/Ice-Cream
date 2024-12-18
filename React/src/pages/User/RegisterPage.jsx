import React, { useState } from "react";
import userServices from "./../../services/userServices";

import "./styles/Auth.css";
const RegisterPage = () => {
  const [userData, setUserData] = useState({
    username: "",
    password: "",
    email: "",
    address: "",
  });
  const [message, setMessage] = useState(null);
  
  const [error, setError] = useState(null);

  const [usernameErr, setUsernameErr] = useState(false);

  const [emailErr, setEmailErr] = useState(false);

  const [pwdError, setPwdError] = useState(false);

  const [addressErr,setAddressError] = useState(false);

  const validUsername = new RegExp('^[A-Za-z0-9]{2,18}$');

  const validEmail = new RegExp('^[a-zA-Z0-9._:$!%-]+@[a-zA-Z0-9.-]+.[a-zA-Z]$');

  const validPassword = new RegExp('^(?=.*?[A-Za-z])(?=.*?[0-9]).{6,}$');

  const validAddress = new RegExp('^[^\\s][a-zA-Z0-9\\s]+$');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUserData({ ...userData, [name]: value });
  };

  const handleSubmit = async (e) => {
    if (!validUsername.test(username)) {
      setUsernameErr(true);
    }
    if (!validPassword.test(password)) {
    setPwdError(true);
    }
    if (!validEmail.test(email)) {
      setEmailErr(true);
    }
    if (!validAddress.test(address)) {
    setAddressError(true);
    }
    e.preventDefault();
    try {
      const data = await userServices.register(userData);
      setMessage(
        "Registration successful! Please check your email to verify your account."
      );
      setError(null);
    } catch (err) {
      console.error("Registration Error:", err); // Log the complete error to the console
      setError(
        err.response?.data?.message || err.message || "Registration failed"
      );
    }
  };

  return (
    <div>
      <div className="col p-4 d-flex flex-column justify-content-center m-0 py-5 animate__animated animate__fadeInDown">
        <h1 className="text-center">Let's Get Started!</h1>
        <p className="text-center text-secondary">
          Create new account to access all features
        </p>
        <div className="row m-0 p-0 justify-content-start justify-content-md-center">
          <div className="col col-md-8">
            <hr />
            <form onSubmit={handleSubmit}>
              <div className="mb-3">
                <label for="username" className="form-label">
                  Username
                </label>
                <input
                  type="text"
                  name="username"
                  placeholder="Username"
                  value={userData.username}
                  onChange={handleChange}
                  required
                />
              </div>
              {usernameErr && <p>Your Username is invalid</p>}
              <div className="mb-3">
                <label for="password" className="form-label">
                  Password
                </label>
                <input
                  type="password"
                  name="password"
                  placeholder="Password"
                  value={userData.password}
                  onChange={handleChange}
                  required
                />
              </div>
              {pwdError && <p>Your Password is invalid</p>}
              <div className="mb-3">
                <label for="email" className="form-label">
                  Email
                </label>
                <input
                  type="email"
                  name="email"
                  placeholder="Email"
                  value={userData.email}
                  onChange={handleChange}
                  required
                />
              </div>
              {emailErr && <p>Your Email is invalid</p>}
              <div className="mb-3">
                <label for="address" className="form-label">
                  Address
                </label>
                <input
                  type="text"
                  name="address"
                  placeholder="Address"
                  value={userData.address}
                  onChange={handleChange}
                  required
                />
              </div>
              {addressErr && <p>Your Address is invalid</p>}
              <div className="d-grid">
                <button
                  type="submit"
                  className="btn"
                  style={{ backgroundColor: "#efc81a", color: "white" }}
                >
                </button>
                {message && <p style={{ color: "green" }}>{message}</p>}
                {error && (
                  <div style={{ color: "red" }}>
                    <p>Registration failed:</p>
                    <pre>{JSON.stringify(error, null, 2)}</pre>{" "}
                  </div>
                )}
              </div>
            </form>
          </div>
        </div>
      </div>
      <h1>Register</h1>
      <form onSubmit={handleSubmit}></form>
    </div>
  );
};

export default RegisterPage;
