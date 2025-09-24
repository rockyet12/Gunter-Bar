import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./pages/Login";
import Tragos from "./pages/Tragos"; // Importa el nuevo componente

const App = () => {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/tragos" element={<Tragos />} /> {/* Agrega esta línea */}
      </Routes>
    </Router>
  );
};

export default App;