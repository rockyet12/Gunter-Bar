import { useEffect, useState } from 'react';
import { Box, Typography, Container, CircularProgress } from '@mui/material';
import { DataGrid } from '@mui/x-data-grid';
import styled from 'styled-components';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import apiService from '../services/apiService';
import { User, UserRole } from '../types';

export default function EmpleoJefeVentas() {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true);
    apiService.getAllUsers()
      .then(res => setUsers(res.data?.filter(u => u.role !== UserRole.SalesManager) || []))
      .catch(() => toast.error('Error al cargar usuarios'))
      .finally(() => setLoading(false));
  }, []);

  return (
    <StyledBg>
      <Container maxWidth="md" sx={{ py: 8 }}>
        <Typography variant="h4" color="primary" gutterBottom align="center" sx={{ fontWeight: 700 }}>
          Panel de Empleo - Jefe de Ventas
        </Typography>
        <Typography variant="body1" color="text.secondary" align="center" sx={{ mb: 4 }}>
          Aquí puedes ver y gestionar los empleados (vendedores y clientes).
        </Typography>
        <ToastContainer position="top-center" autoClose={2500} hideProgressBar theme="colored" aria-label="notificación" />
        {loading ? <CircularProgress /> : (
          <Box sx={{ height: 400, mb: 6 }}>
            <DataGrid
              rows={users.map(u => ({ id: u.id, nombre: u.fullName, email: u.email, rol: UserRole[u.role] }))}
              columns={[
                { field: 'id', headerName: 'ID', width: 80 },
                { field: 'nombre', headerName: 'Nombre', width: 180 },
                { field: 'email', headerName: 'Email', width: 220 },
                { field: 'rol', headerName: 'Rol', width: 160 },
              ]}
              pageSizeOptions={[5]}
              initialState={{ pagination: { paginationModel: { pageSize: 5 } } }}
              disableRowSelectionOnClick
            />
          </Box>
        )}
      </Container>
    </StyledBg>
  );
}

const StyledBg = styled.div`
  min-height: 100vh;
  background: linear-gradient(120deg, #f5f7fa 0%, #c3cfe2 100%);
`;