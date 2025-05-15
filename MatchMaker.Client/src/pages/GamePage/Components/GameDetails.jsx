const GameDetails = ({ editing, canEdit, formState, setFormState }) => {
  const handleChange = (field, value) => {
    setFormState(prev => ({ ...prev, [field]: value }));
  };

  return (
    <div className="mb-4">
      <h2 className="text-lg font-semibold mb-2">Matchdetaljer</h2>

      <div className="mb-2">
        <label className="font-bold">Starttid:</label>
        {editing ? (
          <input
            type="datetime-local"
            value={new Date(formState.startTime).toISOString().slice(0, 16)}
            onChange={(e) =>
              handleChange("startTime", new Date(e.target.value).toISOString())
            }
            className="w-full border p-2 rounded"
          />
        ) : (
          <p>{new Date(formState.startTime).toLocaleString("sv-SE")}</p>
        )}
      </div>

      <div className="mb-2">
        <label className="font-bold">Plats:</label>
        {editing ? (
          <input
            type="text"
            value={formState.location}
            onChange={(e) => handleChange("location", e.target.value)}
            className="w-full border p-2 rounded"
          />
        ) : (
          <p>{formState.location}</p>
        )}
      </div>

      <div className="mb-2">
        <label className="font-bold">Matchtyp:</label>
        {editing ? (
          <select
            value={formState.gameType}
            onChange={(e) => handleChange("gameType", e.target.value)}
            className="w-full border p-2 rounded"
          >
            <option value="7v7">7v7</option>
            <option value="9v9">9v9</option>
            <option value="11v11">11v11</option>
          </select>
        ) : (
          <p>{formState.gameType}</p>
        )}
      </div>
    </div>
  );
};

export default GameDetails;
