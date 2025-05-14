import { useEffect, useState } from "react";
import { useAuth } from "../../context/AuthContext/AuthContext";
import teamService from "../../services/teamService";
import gameService from "../../services/gameService";
import userService from "../../services/userService";

const CreateGame = () => {
    const { user } = useAuth();
    const [startTime, setStartTime] = useState("");
    const [gameType, setGameType] = useState("");
    const [location, setLocation] = useState("");
    const [homeTeam, setHomeTeam] = useState(null);
    const [awayTeamId, setAwayTeamId] = useState("");
    const [refereeId, setRefereeId] = useState("");
    const [court, setCourt] = useState("");
    const [timing, setTiming] = useState("");
    const [offensiveConditions, setOffensiveConditions] = useState("");
    const [defensiveConditions, setDefensiveConditions] = useState("");
    const [specialists, setSpecialists] = useState("");
    const [penalties, setPenalties] = useState("");
    const [teams, setTeams] = useState([]);
    const [referees, setReferees] = useState([]);
    

    useEffect(() => {
        const loadTeams = async () => {
            try {
            const allTeams = await teamService.getAllTeams();
            const myTeam = allTeams.find(t => t.id === user?.teamAffiliation);
            setHomeTeam(myTeam);
            const filteredTeams = allTeams.filter(t => t.id !== user?.teamAffiliation);
            setTeams(filteredTeams);
            } catch (err) {
            console.error("Error loading teams", err);
            }
        };

        if (user) loadTeams();
    }, [user]);

    useEffect(() => {
        const loadReferees = async () => {
            try {
                const referees = await userService.getUsersByRole("Referee")
                setReferees(referees);
            } catch (err) {
                console.error("Error loading referees.", err);
            }
        };

        if (user) loadReferees();
    }, [user]);

    const handleSubmit = async (e) => {
    e.preventDefault();

    const gameData = {
        homeTeamId: homeTeam?.id,
        awayTeamId,
        refereeId,
        startTime,
        gameType,
        location,
        conditions: {
        court,
        timing,
        offensiveConditions,
        defensiveConditions,
        specialists,
        penalties
        }
    };

    try {
      await gameService.createGame(gameData);
      alert("Game created successfully!");
    } catch (error) {
      console.error("Error creating game:", error);
      alert(error.message);
    }
  };

    return (
        <form onSubmit={handleSubmit} className="max-w-md mx-auto p-6">
            <h2 className="text-2xl font-bold mb-4 text-gray-800">Planera match</h2>

            <div className="mb-4">
                <label className="block text-gray-700 mb-2">Hemmalag</label>
                <span className="block font-medium text-gray-900">
                {homeTeam ? homeTeam.teamName : "Laddar hemmalag..."}
                </span>
            </div>

            <div className="mb-4">
                <label htmlFor="awayTeam" className="block text-gray-700 mb-2">
                    Bortalag
                </label>
                <select
                id="awayTeam"
                value={awayTeamId}
                onChange={(e) => setAwayTeamId(e.target.value)}
                className="w-full border rounded px-3 py-2"
                required
                >
                <option value="">
                    Välj bortalag
                </option>
                {teams.map((t) => (
                    <option key={t.id} value={t.id}>
                    {t.teamName}
                    </option>
                ))}
                </select>
            </div>

            <div className="mb-4">
            <label htmlFor="referee" className="block text-gray-700 mb-2">Domare</label>
            <select
            id="referee"
            value={refereeId}
            onChange={(e) => setRefereeId(e.target.value)}
            className="w-full border rounded px-3 py-2"
            required
            >
            <option value="">Välj domare</option>
            {referees.map((r) => (
                <option key={r.id} value={r.id}>
                {r.firstName} {r.lastName}
                </option>
            ))}
            </select>
        </div>

      <div className="mb-4">
        <label htmlFor="startTime" className="block text-gray-700 mb-2">Starttid</label>
        <input
          type="datetime-local"
          id="startTime"
          value={startTime}
          onChange={(e) => setStartTime(e.target.value)}
          className="w-full px-3 py-2 border rounded-md"
          required
        />
      </div>

        <div className="mb-4">
            <label className="block text-gray-700 mb-2">
                Matchtyp
            </label>
            <select
                value={gameType}
                onChange={(e) => setGameType(e.target.value)}
                className="w-full px-3 py-2 border rounded-md"
            >
                <option value="7v7">7v7</option>
                <option value="9v9">9v9</option>
                <option value="11v11">11v11</option>
            </select>
        </div>

        <div className="mb-4">
            <label htmlFor="location" className="block text-gray-700 mb-2">
                Plats
            </label>
            <input
            type="text"
            id="location"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
            className="w-full px-3 py-2 border rounded-md"
            required
            />
        </div>
          
        <h3 className="text-lg font-semibold mt-6 mb-2 text-gray-700">Matchförhållanden</h3>

        <div className="mb-4">
        <label htmlFor="court" className="block text-gray-700 mb-2">
            Plan
        </label>
        <input
            type="text"
            id="court"
            value={court}
            onChange={(e) => setCourt(e.target.value)}
            className="w-full px-3 py-2 border rounded-md"
        />
        </div>

        <div className="mb-4">
            <label htmlFor="timing" className="block text-gray-700 mb-2">
                Tidhållning
            </label>
            <input
                type="text"
                id="timing"
                value={timing}
                onChange={(e) => setTiming(e.target.value)}
                className="w-full px-3 py-2 border rounded-md"
            />
        </div>

        <div className="mb-4">
        <label htmlFor="offensiveConditions" className="block text-gray-700 mb-2">
            Offensiva förhållanden
        </label>
        <input
            type="text"
            id="offensiveConditions"
            value={offensiveConditions}
            onChange={(e) => setOffensiveConditions(e.target.value)}
            className="w-full px-3 py-2 border rounded-md"
        />
        </div>

        <div className="mb-4">
        <label htmlFor="defensiveConditions" className="block text-gray-700 mb-2">
            Defensiva förhållanden
        </label>
        <input
            type="text"
            id="defensiveConditions"
            value={defensiveConditions}
            onChange={(e) => setDefensiveConditions(e.target.value)}
            className="w-full px-3 py-2 border rounded-md"
        />
        </div>

        <div className="mb-4">
        <label htmlFor="specialists" className="block text-gray-700 mb-2">Specialister</label>
        <input
            type="text"
            id="specialists"
            value={specialists}
            onChange={(e) => setSpecialists(e.target.value)}
            className="w-full px-3 py-2 border rounded-md"
        />
        </div>

        <div className="mb-4">
        <label htmlFor="penalties" className="block text-gray-700 mb-2">Straff</label>
        <input
            type="text"
            id="penalties"
            value={penalties}
            onChange={(e) => setPenalties(e.target.value)}
            className="w-full px-3 py-2 border rounded-md"
        />
        </div>

      <button
        type="submit"
        className="w-full bg-blue-600 text-white py-2 px-4 rounded-md hover:bg-blue-700"
      >
        Skapa match
      </button>
    </form>
  );
};

export default CreateGame;
