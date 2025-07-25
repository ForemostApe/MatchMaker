import AdministrateTeam from './Components/AdministrateTeam';

const AdminPage = () => {
    return (
        <div>
            <div className="flex flex-row gap-5"><div><button className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded">Administrera lag</button></div><div><button className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded">Administrera användare</button></div></div>
            <AdministrateTeam />
        </div>
    );
}

export default AdminPage;

