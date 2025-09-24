import React, { useState, useEffect } from "react";
import Card from "../components/cards/card";
import Button from "../components/buttons/button";

const Tragos = () => {
  const [tragos, setTragos] = useState([]);
  const [newTrago, setNewTrago] = useState({ nombre: "", descripcion: "" });

  useEffect(() => {
    // Aquí se hará la llamada a la API para obtener los tragos
    // fetchTragos();
  }, []);

  const fetchTragos = async () => {
    try {
      const response = await fetch("http://localhost:5172/api/Trago", {
        headers: {
          // Si usas JWT, aquí deberías incluir el token
          // 'Authorization': `Bearer ${token}`
        }
      });
      if (response.ok) {
        const data = await response.json();
        setTragos(data);
      }
    } catch (error) {
      console.error("Error al obtener tragos:", error);
    }
  };

  const handleAddTrago = async () => {
    try {
      const response = await fetch("http://localhost:5172/api/Trago", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          // 'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(newTrago),
      });
      if (response.ok) {
        // Vuelve a cargar la lista de tragos
        fetchTragos();
        setNewTrago({ nombre: "", descripcion: "" });
      }
    } catch (error) {
      console.error("Error al agregar trago:", error);
    }
  };

  const handleDeleteTrago = async (id) => {
    try {
      const response = await fetch(`http://localhost:5172/api/Trago/${id}`, {
        method: "DELETE",
        headers: {
          // 'Authorization': `Bearer ${token}`
        },
      });
      if (response.ok) {
        // Vuelve a cargar la lista de tragos
        fetchTragos();
      }
    } catch (error) {
      console.error("Error al eliminar trago:", error);
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Gestión de Tragos</h2>
      <div style={{ display: "flex", gap: "20px", flexWrap: "wrap" }}>
        {tragos.map((trago) => (
          <Card
            key={trago.idTrago}
            header={<h3>{trago.nombre}</h3>}
            body={
              <div>
                <p>{trago.descripcion}</p>
                <p>Ingredientes: {trago.ingredientes}</p>
              </div>
            }
            footer={
              <Button
                label="Eliminar"
                onClick={() => handleDeleteTrago(trago.idTrago)}
                type="button"
              />
            }
          />
        ))}
      </div>
      <div style={{ marginTop: "20px" }}>
        <h3>Agregar Nuevo Trago</h3>
        <input
          type="text"
          placeholder="Nombre"
          value={newTrago.nombre}
          onChange={(e) => setNewTrago({ ...newTrago, nombre: e.target.value })}
        />
        <input
          type="text"
          placeholder="Descripción"
          value={newTrago.descripcion}
          onChange={(e) => setNewTrago({ ...newTrago, descripcion: e.target.value })}
        />
        <Button label="Agregar" onClick={handleAddTrago} type="button" />
      </div>
    </div>
  );
};

export default Tragos;