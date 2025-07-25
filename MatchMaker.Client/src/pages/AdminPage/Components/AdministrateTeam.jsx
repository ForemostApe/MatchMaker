import { useState } from "react";
import teamService from "../../../services/teamService";

const AdministrateTeam = () => {

    const [teamName, setTeamName] = useState("");
    const [teamLogoImage, setTeamLogoImage] = useState(null);
    const [status, setStatus] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!teamName) {
            setStatus("Vänligen ange ett lagnamn.")
            return;
        }

        const formData = new FormData();
        formData.append("teamName", teamName);
        formData.append("teamLogo", teamLogoImage);

        try {
            await teamService.createTeam(formData);
            setStatus("Laget sparades.");
        } catch (error) {
            console.error("Error creating team:", error);
            setStatus(error.message || "Något gick fel.");
        }
    };

    return (
        <div>
            <form onSubmit={handleSubmit} className="max-w-md mx-auto p-6">
                <h1 className="text-2xl font-bold mb-4 text-gray-800">
                    Skapa lag
                </h1>
                <div>
                    <label htmlFor="teamName" className="block text-gray-700 mb-2 font-bold">
                        Lagets namn
                    </label>
                    <input
                        type="text"
                        id="teamName"
                        onChange={(e) => setTeamName(e.target.value)}
                        className="w-full px-3 py-2 border rounded-md"
                        required
                    />
                </div>
                <div>
                    <label>Team Logo:</label>
                    <input
                        type="file"
                        accept="image/*"
                        onChange={(e) => setImageFile(e.target.files[0])}
                        required
                        className="mt-4 mb-4 block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100"
                    />
                    {teamLogoImage && (
                        <img
                            src={teamLogoImage}
                            alt="Förhandsvisning av laglogo"
                            className="mt-4 w-32 h-32 object-contain border rounded"
                        />
                    )}
                </div>
                <button type="submit" className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded">Spara</button>
                {status && <p>{status}</p>}
            </form>
        </div>
    )
};

export default AdministrateTeam;