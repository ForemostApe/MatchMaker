import { useState, useEffect } from "react";
import userService from "../../../services/userService";
import { useAuth } from "../../../context/AuthContext/AuthContext";

const formatDateLocal = (dateStr) => {
  if (!dateStr) return "";
  const date = new Date(dateStr);
  const pad = (n) => n.toString().padStart(2, "0");

  const year = date.getFullYear();
  const month = pad(date.getMonth() + 1);
  const day = pad(date.getDate());
  const hours = pad(date.getHours());
  const minutes = pad(date.getMinutes());

  return `${year}-${month}-${day}T${hours}:${minutes}`;
};

const DetailItem = ({ title, value, editable, onChange, field, type = "text", options }) => {
  const renderInput = () => {
    switch (type) {
      case "datetime-local":
        return (
          <input
            type="datetime-local"
            value={formatDateLocal(value)}
            onChange={(e) => onChange(field, e.target.value)}
            className="w-full px-3 py-2 border rounded-md"
          />
        );
      case "select":
        return (
          <select
            value={value || ""}
            onChange={(e) => onChange(field, e.target.value)}
            className="w-full px-3 py-2 border rounded-md"
          >
            {options.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        );
      case "text":
      default:
        return (
          <input
            type={type}
            value={value}
            onChange={(e) => onChange(field, e.target.value)}
            className="w-full px-3 py-2 border rounded-md"
          />
        );
    }
  };

  const renderDisplay = () => {
    if (field === "startTime") {
      return <p className="text-gray-900">{new Date(value).toLocaleString("sv-SE")}</p>;
    }
    if (field === "refereeId") {
      const referee = options.find((r) => r.value === value);
      return <p className="text-gray-900">{referee ? `${referee.label}` : "–"}</p>;
    }
    return <p className="text-gray-900">{value}</p>;
  };

  return (
    <div className="mb-4">
      <label className="block text-gray-700 mb-2 font-bold">{title}</label>
      {editable ? renderInput() : renderDisplay()}
    </div>
  );
};

const GameDetails = ({ editing, canEdit, formState, setFormState }) => {
  const { user } = useAuth();
  const [referees, setReferees] = useState([]);

  const handleChange = (field, value) => {
    setFormState((prev) => ({ ...prev, [field]: value }));
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

  const gameTypeOptions = [
    { value: "7v7", label: "7v7" },
    { value: "9v9", label: "9v9" },
    { value: "11v11", label: "11v11" }
  ];

  const refereeOptions = [
    { value: "", label: "Välj domare" },
    ...referees.map((r) => ({
      value: r.id,
      label: `${r.firstName} ${r.lastName}`
    }))
  ];

  return (
    <div className="max-w-md mx-auto p-6">
      <h2 className="text-2xl font-bold mb-4 text-gray-800">Matchdetaljer</h2>

      <DetailItem
        title="Starttid"
        value={formState.startTime}
        editable={canEdit && editing}
        onChange={handleChange}
        field="startTime"
        type="datetime-local"
      />

      <DetailItem
        title="Plats"
        value={formState.location}
        editable={canEdit && editing}
        onChange={handleChange}
        field="location"
      />

      <DetailItem
        title="Matchtyp"
        value={formState.gameType}
        editable={canEdit && editing}
        onChange={handleChange}
        field="gameType"
        type="select"
        options={gameTypeOptions}
      />

      <DetailItem
        title="Domare"
        value={formState.refereeId}
        editable={canEdit && editing}
        onChange={handleChange}
        field="refereeId"
        type="select"
        options={refereeOptions}
      />
    </div>
  );
};

export default GameDetails;
