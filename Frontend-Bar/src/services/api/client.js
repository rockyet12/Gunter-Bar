export const api = {
  baseUrl: import.meta.env.VITE_API_URL || 'http://localhost:5226',
};

export async function http(path, options = {}) {
  const res = await fetch(`${api.baseUrl}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(options.headers || {}),
    },
    ...options,
  });
  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || `HTTP ${res.status}`);
  }
  const contentType = res.headers.get('content-type');
  if (contentType && contentType.includes('application/json')) return res.json();
  return res.text();
}
