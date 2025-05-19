import { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext/AuthContext";
import teamService from "../../services/teamService";
import userService from "../../services/userService";

const ProfilePage = () => {
  const { user, isLoading, refreshUser } = useAuth();
  const [team, setTeam] = useState(null);
  const [teams, setTeams] = useState([]);
  const [isEditing, setIsEditing] = useState(false);

  const [formData, setFormData] = useState({
    id: "",
    firstName: "",
    lastName: "",
    userRole: "",
    teamAffiliation: null,
  });

  const roleMapping = {
    Admin: "Administratör",
    Coach: "Tränare",
    Referee: "Domare",
    Functionary: "Funktionär",
    Guest: "Gäst",
    Unspecified: "Ej angiven",
  };

useEffect(() => {
  const loadData = async () => {
    try {
      if (user) {
        const allTeams = await teamService.getAllTeams();
        setTeams(allTeams);

        const updatedFormData = {
          id: user.id,
          firstName: user.firstName,
          lastName: user.lastName,
          userRole: user.userRole,
          teamAffiliation: user.teamAffiliation || null,
        };
        setFormData(updatedFormData);

      if (user.teamAffiliation) {
        try {
          const teamData = await teamService.getTeamById(user.teamAffiliation);
          setTeam(teamData);
        } catch (err) {
          console.error("Kunde inte hämta laget:", err);
          setTeam(null);
        }
      } else {
        setTeam(null);
      }
            }
          } catch (error) {
            console.error("Error loading profile data", error);
          }
        };

  loadData();
}, [user]);

const handleChange = (e) => {
  const { name, value } = e.target;

  setFormData(prev => ({
    ...prev,
    [name]: name === "teamAffiliation" && value === "" ? null : value,
  }));
};

  const handleSave = async () => {
    try {
      await userService.updateUser(formData);
      setIsEditing(false);
      await refreshUser();
    } catch (error) {
      console.error("Failed to update profile", error);
    }
  };

  const handleCancel = () => {
    if (user) {
      setFormData({
        id: user.id,
        firstName: user.firstName,
        lastName: user.lastName,
        userRole: user.userRole,
        teamAffiliation: user.teamAffiliation || null,
      });
    }
    setIsEditing(false);
  };

  if (isLoading || !user) {
    return (
      <div className="flex justify-center items-center h-screen text-gray-600">
        Laddar användarprofil...
      </div>
    );
  }

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-white p-4 sm:p-8">
      <div className="w-full max-w-xl rounded-lg shadow-md p-6 sm:p-8 bg-gray-100">
        <h1 className="text-2xl sm:text-3xl font-bold text-center mb-6">Min Profil</h1>

        <div className="space-y-4 text-sm sm:text-base">
          <div>
            <span className="font-bold">E-post: </span>
            <span>{user.email}</span>
          </div>

          <div>
            <span className="font-bold">Förnamn: </span>
            {isEditing ? (
              <input
                type="text"
                name="firstName"
                value={formData.firstName}
                onChange={handleChange}
                className="border rounded p-1 ml-2"
              />
            ) : (
              <span>{user.firstName}</span>
            )}
          </div>

          <div>
            <span className="font-bold">Efternamn: </span>
            {isEditing ? (
              <input
                type="text"
                name="lastName"
                value={formData.lastName}
                onChange={handleChange}
                className="border rounded p-1 ml-2"
              />
            ) : (
              <span>{user.lastName}</span>
            )}
          </div>

          <div>
            <span className="font-bold">Roll: </span>
            {isEditing ? (
              <select
                name="userRole"
                value={formData.userRole}
                onChange={handleChange}
                className="border rounded p-1 ml-2"
              >
                <option value="">Välj roll</option>
                <option value="Coach">Tränare</option>
                <option value="Referee">Domare</option>
                <option value="Functionary">Funktionär</option>
                <option value="Guest">Gäst</option>
              </select>
            ) : (
              <span>{roleMapping[user.userRole] || user.userRole}</span>
            )}
          </div>

          <div>
            <span className="font-bold">Lagtillhörighet: </span>
            {isEditing ? (
              <select
                name="teamAffiliation"
                value={formData.teamAffiliation ?? ""}
                onChange={handleChange}
                className="border rounded p-1 ml-2"
              >
                <option value="">Ingen</option>
                {teams.map((t) => (
                  <option key={t.id} value={t.id}>
                    {t.teamName}
                  </option>
                ))}
              </select>
            ) : (
              <span>{!formData.teamAffiliation? "Inget tilldelat lag" : !team? "Laddar lag..." : team.teamName}</span>
            )}
          </div>
        </div>

        <div className="mt-6 flex justify-end gap-4">
          {isEditing ? (
            <>
              <button
                onClick={handleSave}
                className="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded"
              >
                Spara
              </button>
              <button
                onClick={handleCancel}
                className="bg-gray-400 hover:bg-gray-500 text-white px-4 py-2 rounded"
              >
                Avbryt
              </button>
            </>
          ) : (
            <button
              onClick={() => setIsEditing(true)}
              className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded"
            >
              Redigera
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProfilePage;