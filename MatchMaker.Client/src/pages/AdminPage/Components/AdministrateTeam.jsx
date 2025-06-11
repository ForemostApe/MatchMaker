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

        const formData = new formData();
        formData.append("teamName", teamName);
        formData.append("teamLogo", teamLogoImage);

        try {
            await teamService.createTeam(teamData);
            setStatus("Laget sparades.");

        } catch (error) {
            console.error("Error creating team:", error);
            setMessage(error.message || "Något gick fel.");
            setMessageType("error");
        }
    };

    return (
        <div>
            <form onSubmit={handleSubmit} className="max-w-md mx-auto p-6">
                <h1 className="text-2xl font-bold mb-4 text-gray-800">
                    Skapa lag
                </h1>
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
                <label>Team Logo:</label>
                <input
                    type="file"
                    accept="image/*"
                    onChange={(e) => setImageFile(e.target.files[0])}
                    required
                />
                <button type="submit">Spara</button>
                {status && <p>{status}</p>}
            </form>
        </div>
    )
};

export default AdministrateTeam;