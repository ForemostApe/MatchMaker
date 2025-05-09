import { Outlet } from "react-router";
import Navbar from '../components/Navbar/Navbar';
import { useAuth } from "../context/AuthContext/AuthContext";

const Layout = () => {
    const { user, isLoading, error } = useAuth();

    if (isLoading) return <div>Loading...</div>;

    return (
        <div className="min-h-screen flex flex-col">
            <main className="flex-1 p-4 md:p-6 lg:p-8">
                <div className="mx-auto w-full max-w-7xl bg-white rounded-lg border border-gray-200 p-4 md:p-6 lg:p-8">
                    <Navbar user={user.userRole} />
                    <Outlet />
                </div>
            </main>
        </div>
    );
};

export default Layout;
