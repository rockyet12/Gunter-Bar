import React from "react";

const Button = ({ label, onClick, type }) => {
  return (
    <button
      type="{type}"
      onClick={onClick}
      style={{
        padding: "10px 20px",
        backgroundColor: "#007BFF",
        color: "#fff",
        border: "none",
        borderRadius: "5px",
        cursor: "pointer",
        fontSize: "16px",
      }}
    >
      {label}
    </button>
  );
};

export default Button;
