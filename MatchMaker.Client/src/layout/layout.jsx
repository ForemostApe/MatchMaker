import { Outlet } from "react-router";
import Navbar from "../components/Navbar/Navbar";

const Layout = () => {
    return(
        <div className="min-h-screen flex flex-col">
                <main>
                    <div className="flex-1 p-4 md:p-6 lg:p-8">
                        <div className="mx-auto w-full max-w-7xl bg-white rounded-lg border border-gray-200 p-4 md:p-6 lg:p-8">
                            <Outlet />
                        </div>
                    </div>
                </main>

        </div>
    )
};

export default Layout;