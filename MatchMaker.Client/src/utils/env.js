export function getApiBase() {
    const base = import.meta.env.VITE_API_BASE;
    if (!base) throw new Error("Missing VITE_API_BASE");
    return base;
  }