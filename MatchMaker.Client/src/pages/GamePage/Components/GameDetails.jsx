import { useState, useEffect } from "react";
import userService from "../../../services/userService";
import { useAuth } from "../../../context/AuthContext/AuthContext";

const GameDetails = ({ editing, canEdit, formState, setFormState }) => {
  const { user } = useAuth();
  const [referees, setReferees] = useState([]);

  const handleChange = (field, value) => {
    setFormState(prev => ({ ...prev, [field]: value }));
  };

  useEffect(() => {
    const loadReferees = async () => {
      try {
        const referees = await userService.getUsersByRole("Referee");
        setReferees(referees);
      } catch (err) {
        console.error("Error loading referees.", err);
      }
    };

    if (user) loadReferees();
  }, [user]);

  return (
    <div className="max-w-md mx-auto p-6">
      <h2 className="text-2xl font-bold mb-4 text-gray-800">Matchdetaljer</h2>

      <div className="mb-4">
        <label className="block text-gray-700 mb-2 font-bold">Starttid</label>
        {editing ? (
          <input
            type="datetime-local"
            value={new Date(formState.startTime).toISOString().slice(0, 16)}
            onChange={(e) =>
              handleChange("startTime", new Date(e.target.value).toISOString())
            }
            className="w-full px-3 py-2 border rounded-md"
          />
        ) : (
          <p className="text-gray-900">
            {new Date(formState.startTime).toLocaleString("sv-SE")}
          </p>
        )}
      </div>

      <div className="mb-4">
        <label className="block text-gray-700 mb-2 font-bold">Plats</label>
        {editing ? (
          <input
            type="text"
            value={formState.location}
            onChange={(e) => handleChange("location", e.target.value)}
            className="w-full px-3 py-2 border rounded-md"
          />
        ) : (
          <p className="text-gray-900">{formState.location}</p>
        )}
      </div>

      <div className="mb-4">
        <label className="block text-gray-700 mb-2 font-bold">Matchtyp</label>
        {editing ? (
          <select
            value={formState.gameType}
            onChange={(e) => handleChange("gameType", e.target.value)}
            className="w-full px-3 py-2 border rounded-md"
          >
            <option value="7v7">7v7</option>
            <option value="9v9">9v9</option>
            <option value="11v11">11v11</option>
          </select>
        ) : (
          <p className="text-gray-900">{formState.gameType}</p>
        )}
      </div>

      <div className="mb-4">
        <label htmlFor="referee" className="block text-gray-700 mb-2 font-bold">Domare</label>
        {editing ? (
          <select
            id="referee"
            value={formState.refereeId || ""}
            onChange={(e) => handleChange("refereeId", e.target.value)}
            className="w-full border rounded px-3 py-2"
          >
            <option value="">Välj domare</option>
            {referees.map((r) => (
              <option key={r.id} value={r.id}>
                {r.firstName} {r.lastName}
              </option>
            ))}
          </select>
        ) : (
          <p className="text-gray-900">
            {referees.find(r => r.id === formState.refereeId)?.firstName || "–"}{" "}
            {referees.find(r => r.id === formState.refereeId)?.lastName || ""}
          </p>
        )}
      </div>
    </div>
  );
};

export default GameDetails;
