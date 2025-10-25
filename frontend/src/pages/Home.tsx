import React, { useState, useEffect } from 'react';
import { Box, Typography, Container, Grid, Card, CardMedia, CardContent, CardActionArea, TextField, InputAdornment, Skeleton, Button, Fade, Chip } from '@mui/material';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import styled from 'styled-components';
import AOS from 'aos';
import 'aos/dist/aos.css';
import LocalBarIcon from '@mui/icons-material/LocalBar';
import './Home.css';
import { useAuth } from '../features/auth/contexts/AuthContext';
import SearchIcon from '@mui/icons-material/Search';

const bebidas = [
  {
    value: 'mojito',
    label: 'Mojito',
    img: 'https://images.unsplash.com/photo-1504674900247-0877df9cc836?auto=format&fit=crop&w=400&q=80',
    desc: 'Refrescante cóctel cubano con menta, lima y ron.'
  },
  {
    value: 'caipirinha',
    label: 'Caipirinha',
    img: 'https://images.unsplash.com/photo-1514361892635-cebb9b6b9d49?auto=format&fit=crop&w=400&q=80',
    desc: 'El clásico brasileño con cachaça, lima y azúcar.'
  },
  {
    value: 'negroni',
    label: 'Negroni',
    img: 'https://images.unsplash.com/photo-1464306076886-debca5e8a6b0?auto=format&fit=crop&w=400&q=80',
    desc: 'Bebida italiana con gin, vermut y Campari.'
  },
  {
    value: 'fernet',
    label: 'Fernet con Coca',
    img: 'https://images.unsplash.com/photo-1502741338009-cac2772e18bc?auto=format&fit=crop&w=400&q=80',
    desc: 'El clásico argentino: Fernet Branca y Coca-Cola.'
  },
  {
    value: 'cerveza',
    label: 'Cerveza',
    img: 'https://images.unsplash.com/photo-1513104890138-7c749659a591?auto=format&fit=crop&w=400&q=80',
    desc: 'Rubia, roja o negra, siempre fría.'
  },
];

export default function Home() {
  const { user } = useAuth();
  const [selected, setSelected] = useState<null | typeof bebidas[0]>(null);
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(false);
  const [bebidasFiltradas, setBebidasFiltradas] = useState(bebidas);
  const [suggestions, setSuggestions] = useState<typeof bebidas>([]);

  useEffect(() => {
    AOS.init({ duration: 900, once: true });
  }, []);

  useEffect(() => {
    setLoading(true);
    const timeout = setTimeout(() => {
      const filtradas = bebidas.filter(b =>
        b.label.toLowerCase().includes(search.toLowerCase()) ||
        b.desc.toLowerCase().includes(search.toLowerCase())
      );
      setBebidasFiltradas(filtradas);
      // Sugerencias si no hay resultados exactos
      if (filtradas.length === 0 && search.length > 0) {
        const sugeridas = bebidas.filter(b =>
          b.label.toLowerCase().split(' ').some(word => search.toLowerCase().includes(word))
        );
        setSuggestions(sugeridas);
      } else {
        setSuggestions([]);
      }
      setLoading(false);
    }, 400);
    return () => clearTimeout(timeout);
  }, [search]);

  return (
    <StyledBg>
      <Container maxWidth="md" sx={{ py: 6 }}>
      <Box sx={{ textAlign: 'center', mb: 4 }}>
        <Typography className="gunter-title animated-title" variant="h4" color="primary" sx={{ fontWeight: 700, letterSpacing: 1, display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 1 }}>
          <LocalBarIcon sx={{ fontSize: 36, color: 'secondary.main' }} />
          ¡Bienvenido{user ? `, ${user.name}` : ''} a Gunter Bar!
        </Typography>
        <Typography variant="subtitle1" color="text.secondary" sx={{ mb: 2 }}>
          Elige tu bebida favorita
        </Typography>
      </Box>
      <Box sx={{ maxWidth: 400, mx: 'auto', mb: 4 }}>
        <TextField
          fullWidth
          placeholder="Buscar bebida..."
          value={search}
          onChange={e => setSearch(e.target.value)}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon />
              </InputAdornment>
            ),
          }}
        />
      </Box>
  <Grid container spacing={3} justifyContent="center">
        {loading ? (
          Array.from({ length: 4 }).map((_, i) => (
            <Grid item xs={12} sm={6} md={4} key={i}>
              <Skeleton variant="rectangular" height={180} />
            </Grid>
          ))
        ) : bebidasFiltradas.length > 0 ? (
          bebidasFiltradas.map(b => (
            <Grid item xs={12} sm={6} md={4} key={b.value} data-aos="fade-up">
              <Card className="shadow-sm border border-light animated-card" style={{ borderRadius: 16 }}>
                <CardActionArea>
                  <CardMedia
                    component="img"
                    height="140"
                    image={b.img}
                    alt={b.label}
                  />
                  <CardContent>
                    <Typography variant="h6" sx={{ fontWeight: 600 }}>{b.label}</Typography>
                    <Typography variant="body2" color="text.secondary">{b.desc}</Typography>
                  </CardContent>
                </CardActionArea>
              </Card>
            </Grid>
          ))
        ) : (
          <Grid item xs={12}>
            <Typography variant="body1" color="text.secondary" align="center" sx={{ mt: 4 }}>
              No se encontraron bebidas. Prueba otra búsqueda.
            </Typography>
          </Grid>
        )}
      </Grid>
  <ToastContainer position="bottom-right" autoClose={2000} theme="colored" aria-label="notificación" />
    </Container>
    </StyledBg>
  );
}

// Fondo con styled-components y Bootstrap
const StyledBg = styled.div`
  min-height: 100vh;
  background: linear-gradient(120deg, #f5f7fa 0%, #c3cfe2 100%);
  font-family: 'Montserrat', 'Roboto', 'Arial', sans-serif;
`;
