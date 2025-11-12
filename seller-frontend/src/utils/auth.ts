export const getToken = () => localStorage.getItem('gunter_token');
export const getRole = () => localStorage.getItem('gunter_role');
export const isSeller = () => getRole() === 'seller';
export const logout = () => {
  localStorage.removeItem('gunter_token');
  localStorage.removeItem('gunter_role');
  window.location.href = 'http://localhost:5173';
};