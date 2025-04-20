import React, { useState } from "react";
import Card from "../components/cards/card";
import Button from "../components/buttons/button";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = () => {
    fetch("http://localhost:5172/api/Usuario/Login", {
      method: "POST",
        mode: "cors",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, password }),
    })
      .then((response) => response.json())
      .then((data) => {
        if (data.success) {
          alert("Inicio de sesión exitoso!");
          // Aquí puedes redirigir al usuario a otra página o realizar otras acciones
        } else {
          alert("Error en el inicio de sesión. Intenta nuevamente.");
        }
      })
      .catch((error) => {
        console.error("Error:", error);
        alert("Ocurrió un error. Intenta nuevamente.");
      });
  };

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
      }}
    >
      <Card
        header={<h2>Iniciar Sesión</h2>}
        body={
          <div
            style={{ display: "flex", flexDirection: "column", gap: "10px" }}
          >
            <input
              type="text"
              placeholder="Usuario"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              style={{
                padding: "10px",
                border: "1px solid #ccc",
                borderRadius: "5px",
                fontSize: "16px",
              }}
            />
            <input
              type="password"
              placeholder="Contraseña"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              style={{
                padding: "10px",
                border: "1px solid #ccc",
                borderRadius: "5px",
                fontSize: "16px",
              }}
            />
          </div>
        }
        footer={
          <Button label="Iniciar Sesión" onClick={handleLogin} type="button" />
        }
      />
    </div>
  );
};

export default Login;
