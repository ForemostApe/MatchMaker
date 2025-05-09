import { useEffect, useState } from "react";
import { format } from "date-fns";
import { sv } from "date-fns/locale";
import { useAuth } from "../../context/AuthContext/AuthContext";
import teamService from "../../services/teamService";

const ProfilePage = () => {
  const { user, isLoading } = useAuth(); // Use the AuthContext
  const [team, setTeam] = useState(null);

  useEffect(() => {
    const fetchTeam = async () => {
      try {
        if (user?.teamAffiliation) {
          const teamData = await teamService.getTeamById(user.teamAffiliation);
          setTeam(teamData);
        }
      } catch (error) {
        console.error("Error fetching team info:", error);
      }
    };

    if (user) {
      fetchTeam();
    }
  }, [user]);

  if (isLoading || !user) {
    return (
      <div className="flex justify-center items-center h-screen text-gray-600">
        Laddar användarprofil...
      </div>
    );
  }

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 p-4 sm:p-8">
      <div className="w-full max-w-xl bg-white rounded-lg shadow-md p-6 sm:p-8">
        <h1 className="text-2xl sm:text-3xl font-bold text-center mb-6">Min Profil</h1>

        <div className="space-y-4 text-sm sm:text-base">
            <div>
                <span className="font-bold">E-post: </span><span>{user.email}</span>
            </div>
            <div>
                <span className="font-bold">Förnamn: </span>
                <span>{user.firstName}</span>
            </div>
            <div>
                <span className="font-bold">Efternamn: </span>
                <span>{user.lastName}</span>
            </div>
            <div>
                <span className="font-bold">Roll: </span>
                <span>{user.userRole}</span>
            </div>
            <div>
                <span className="font-bold">Lagtillhörighet: </span>
                <span>{team ? team.teamName : "Inget tilldelat lag"}</span>
            </div>
        </div>
      </div>
    </div>
  );
};

export default ProfilePage;
