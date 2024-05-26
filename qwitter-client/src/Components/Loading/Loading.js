import React from "react";
import loading from "./loading.svg";
import './Loading.css';

const Loading = () => (
  <div className="loading-container">
    <div className="spinner">
      <img src={loading} alt="Loading" />
    </div>
  </div>
);

export default Loading;