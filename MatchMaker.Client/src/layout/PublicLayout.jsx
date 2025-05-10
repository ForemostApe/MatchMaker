import { Outlet } from "react-router-dom";

const PublicLayout = () => {
  return (
    <div className="min-h-screen flex flex-col">
        <main className="flex-1 p-4 md:p-6 lg:p-8">
            <div className="mx-auto w-full max-w-7xl bg-white rounded-lg border border-gray-200 p-4 md:p-6 lg:p-8 bg-amber-300">
                <Outlet />
            </div>
        </main>
    </div>
  );
};

export default PublicLayout;