import { useParams, useLocation,useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import useGameData from "../../hooks/useGameData";
import GameHeader from "./Components/GameHeader";
import GameConditions from "./Components/GameConditions";
import GameDetails from "./Components/GameDetails";
import { useAuth } from "../../context/AuthContext/AuthContext";
import gameService from "../../services/gameService";
import GameResponse from "./Components/GameResponse";

const GamePage = () => {
  const { id } = useParams();
  const location = useLocation();
  const { game, homeTeam, awayTeam, loading, error } = useGameData(id, location);
  const { user } = useAuth();
  const [editing, setEditing] = useState(false);
  const [formState, setFormState] = useState(null);
  const [saving, setSaving] = useState(false);
  const [saveError, setSaveError] = useState(null);
  const [message, setMessage] = useState("");
  const [messageType, setMessageType] = useState("");
  const navigate = useNavigate();

  const canEdit = user.userRole === "Coach" && user.teamAffiliation === game?.homeTeamId;

  useEffect(() => {
    if (game) {
      setFormState({
        startTime: game.startTime,
        location: game.location,
        gameType: game.gameType,
        refereeId: game.refereeId,
        conditions: { ...game.conditions }
      });
    }
  }, [game]);

  useEffect(() => {
  if (message) {
    const timer = setTimeout(() => {
      setMessage("");
    }, 3000);
    return () => clearTimeout(timer);
  }
}, [message]);

  const toggleEditing = () => {
    if (!editing) {
      setFormState({
        startTime: game.startTime,
        location: game.location,
        gameType: game.gameType,
        refereeId: game.refereeId,
        conditions: { ...game.conditions }
      });
    }
    setEditing(prev => !prev);
    setSaveError(null);
  };

  const handleCancel = () => {
    toggleEditing();
  };

const handleSave = async () => {
  setSaving(true);
  setSaveError(null);
  try {
    await gameService.updateGame({
      id: game.id,
      startTime: formState.startTime,
      location: formState.location,
      gameType: formState.gameType,
      refereeId: formState.refereeId,
      conditions: formState.conditions
    });
    setEditing(false);
    setMessage("Matchen har uppdaterats!");
    setMessageType("success");

    setTimeout(() => {
      setMessage("");
      navigate("/");
    }, 2000);
  } catch (err) {
    setSaveError(err.message);
    setMessage("Misslyckades att spara matchen.");
    setMessageType("error");
  } finally {
    setSaving(false);
  }
};

  if (loading) return <p>Loading...</p>;
  if (error || !formState) return <p>Error loading game data.</p>;

  return (
    <div className="p-4 max-w-3xl mx-auto bg-gray-50 shadow rounded">
      <GameHeader homeTeam={homeTeam} awayTeam={awayTeam} game={game} />

      <GameDetails
        editing={editing}
        canEdit={canEdit}
        formState={formState}
        setFormState={setFormState}
      />

      <GameConditions
        editing={editing}
        canEdit={canEdit}
        formState={formState}
        setFormState={setFormState}
      />

      {canEdit && (
        <div className="flex gap-2 mt-4">
          {!editing ? (
            <button
              className="px-4 py-2 bg-blue-600 text-white rounded"
              onClick={toggleEditing}
            >
              Redigera
            </button>
          ) : (
            <>
              <button
                className="px-4 py-2 bg-green-600 text-white rounded disabled:opacity-50"
                onClick={handleSave}
                disabled={saving}
              >
                {saving ? "Sparar..." : "Spara"}
              </button>
              <button
                className="px-4 py-2 bg-gray-500 text-white rounded"
                onClick={handleCancel}
              >
                Avbryt
              </button>
              {saveError && <p className="text-red-600">{saveError}</p>}
            </>
          )}
        </div>
      )}

      {message && (
        <div
          className={`mt-4 p-4 rounded text-white text-center ${
            messageType === "success" ? "bg-green-600" : "bg-red-600"
          }`}
        >
          {message}
        </div>
      )}

      <GameResponse game={game} />
      
    </div>
  );
};

export default GamePage;
