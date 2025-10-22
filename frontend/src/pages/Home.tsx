import { Box, Typography, Container, Grid, Card, CardMedia, CardContent, CardActionArea, TextField, InputAdornment, Skeleton, Button, Fade, Chip } from '@mui/material';
import { useAuth } from '../features/auth/contexts/AuthContext';
import React, { useState } from 'react';
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

  React.useEffect(() => {
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
    <Container maxWidth="lg" sx={{ py: 8 }}>
      <Typography variant="h3" color="primary" gutterBottom align="center" sx={{ fontWeight: 700, letterSpacing: 1 }}>
        ¡Bienvenido{user ? `, ${user.name}` : ''} a Gunter Bar!
      </Typography>
      <Typography variant="h5" color="secondary" sx={{ mb: 4, fontWeight: 500 }} align="center">
        Elige tu bebida favorita
      </Typography>
      {!selected && (
        <Box sx={{ maxWidth: 400, mx: 'auto', mb: 4 }}>
          <TextField
            fullWidth
            variant="outlined"
            placeholder="Buscar bebida..."
            value={search}
            onChange={e => setSearch(e.target.value)}
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <SearchIcon color="primary" />
                </InputAdornment>
              ),
              sx: { bgcolor: '#fff', borderRadius: 2, fontSize: '1.1rem' }
            }}
            sx={{ fontSize: '1.1rem', mb: 1 }}
          />
          {suggestions.length > 0 && (
            <Box sx={{ mt: 1, textAlign: 'center' }}>
              <Typography variant="body2" color="text.secondary">¿Buscabas alguna de estas?</Typography>
              {suggestions.map(s => (
                <Chip key={s.value} label={s.label} onClick={() => setSearch(s.label)} sx={{ m: 0.5, cursor: 'pointer' }} />
              ))}
            </Box>
          )}
        </Box>
      )}
      {selected ? (
        <Fade in={!!selected}>
          <Box sx={{ maxWidth: 400, mx: 'auto', mb: 4 }}>
            <Card sx={{ boxShadow: 6 }}>
              <CardMedia
                component="img"
                height="260"
                image={selected.img}
                alt={selected.label}
              />
              <CardContent>
                <Typography variant="h5" gutterBottom sx={{ fontWeight: 600 }}>{selected.label}</Typography>
                <Typography variant="body1" color="text.secondary" sx={{ mb: 2 }}>{selected.desc}</Typography>
                <Box sx={{ mt: 2, textAlign: 'center' }}>
                  <Button onClick={() => setSelected(null)} variant="contained" color="primary" sx={{ borderRadius: 2, px: 4, fontWeight: 600 }}>
                    Volver
                  </Button>
                </Box>
              </CardContent>
            </Card>
          </Box>
        </Fade>
      ) : (
        <Grid container spacing={4} justifyContent="center">
          {loading ? (
            Array.from({ length: 4 }).map((_, i) => (
              <Grid item xs={12} sm={6} md={4} lg={3} key={i}>
                <Card sx={{ boxShadow: 4 }}>
                  <Skeleton variant="rectangular" height={180} />
                  <CardContent>
                    <Skeleton variant="text" width="60%" />
                    <Skeleton variant="text" width="80%" />
                  </CardContent>
                </Card>
              </Grid>
            ))
          ) : bebidasFiltradas.length === 0 ? (
            <Grid item xs={12}>
              <Typography variant="body1" color="text.secondary" align="center" sx={{ mt: 4 }}>
                No se encontraron bebidas para tu búsqueda.
              </Typography>
            </Grid>
          ) : (
            bebidasFiltradas.map(bebida => (
              <Grid item xs={12} sm={6} md={4} lg={3} key={bebida.value}>
                <Fade in={!loading}>
                  <Card sx={{ transition: 'transform 0.2s', '&:hover': { transform: 'scale(1.04)', boxShadow: 8 } }}>
                    <CardActionArea onClick={() => setSelected(bebida)}>
                      <CardMedia
                        component="img"
                        height="180"
                        image={bebida.img}
                        alt={bebida.label}
                      />
                      <CardContent>
                        <Typography variant="h6" gutterBottom sx={{ fontWeight: 600 }}>{bebida.label}</Typography>
                        <Typography variant="body2" color="text.secondary">{bebida.desc}</Typography>
                      </CardContent>
                    </CardActionArea>
                  </Card>
                </Fade>
              </Grid>
            ))
          )}
        </Grid>
      )}
    </Container>
  );
}
