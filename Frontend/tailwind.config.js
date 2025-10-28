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
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/container-queries'),
  ],
}
