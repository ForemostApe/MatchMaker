import { Outlet } from "react-router-dom";
import Navbar from "../components/Navbar/Navbar";
import { useAuth } from "../context/AuthContext/AuthContext";

const AuthLayout = () => {
  const { user, isLoading } = useAuth();

  if (isLoading) return <div>Loading...</div>;

  return (
    <div className="min-h-screen flex flex-col">
      <Navbar user={user?.userRole} />
      <main className="flex-1 p-4 md:p-6 lg:p-8">
        <div className="mx-auto w-full max-w-7xl bg-white rounded-lg border border-gray-200 p-4 md:p-6 lg:p-8">
          <Outlet />
        </div>
      </main>
    </div>
  );
};

export default AuthLayout;