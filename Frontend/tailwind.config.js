/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  darkMode: "class",
  theme: {
    extend: {
      colors: {
        "primary": "#F26419", // Naranja
        "secondary": "#4F000B", // Morado vino
        "accent": "#588157", // Verde
        "background-light": "#f8f7f6", // Mantenido por si se cambia a modo claro
        "background-dark": "#1A1A1A", // Fondo oscuro más neutro
        "surface-dark": "#242424", // Superficie para el contenido principal
        "text-light": "#F2F2F2", // Texto principal claro
        "text-muted": "#A3A3A3" // Texto secundario
      },
      fontFamily: {
        "display": ["Epilogue", "sans-serif"]
      },
      borderRadius: {
        "DEFAULT": "0.25rem",
        "lg": "0.5rem",
        "xl": "0.75rem",
        "full": "9999px"
      },
      keyframes: {
        fadeIn: {
          '0%': { opacity: '0', transform: 'translateY(20px)' },
          '100%': { opacity: '1', transform: 'translateY(0)' },
        },
        shake: {
          '0%, 100%': { transform: 'translateX(0)' },
          '25%': { transform: 'translateX(-5px)' },
          '75%': { transform: 'translateX(5px)' },
        },
        scaleIn: {
          '0%': { opacity: '0', transform: 'scale(0.8)' },
          '100%': { opacity: '1', transform: 'scale(1)' },
        },
      },
      animation: {
        fadeIn: 'fadeIn 0.8s ease-out',
        shake: 'shake 0.5s ease-in-out',
        scaleIn: 'scaleIn 0.6s ease-out',
      },
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/container-queries'),
  ],
}
