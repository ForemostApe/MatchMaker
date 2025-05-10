import { Outlet } from "react-router-dom";
import { useEffect } from "react";
import { useAuth } from "../context/AuthContext/AuthContext";
import Navbar from "../components/Navbar/Navbar";

const AuthLayout = () => {
  const { isLoading, refresh, user } = useAuth();

  useEffect(() => {
    if (!user) {
      refresh();
    }
  }, [refresh, user]);

  if (isLoading) return <div>Loading...</div>;

  return (
    <div className="min-h-screen flex flex-col">
      <Navbar />
      <main className="flex-1 p-4 md:p-6 lg:p-8">
        <div className="mx-auto w-full max-w-7xl bg-white rounded-lg border border-gray-200 p-4 md:p-6 lg:p-8">
          <Outlet />
        </div>
      </main>
    </div>
  );
};

export default AuthLayout;