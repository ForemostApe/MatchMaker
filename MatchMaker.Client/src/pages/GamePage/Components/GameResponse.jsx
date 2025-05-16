import { useState } from "react";
import { useAuth } from "../../../context/AuthContext/AuthContext";
import gameService from "../../../services/gameService";

const GameResponse = ({ game }) => {
  const { user } = useAuth();
  const [accepted, setAccepted] = useState(null);
  const [saving, setSaving] = useState(false);
  const [saveError, setSaveError] = useState(null);
  const [submitted, setSubmitted] = useState(false);

  const isCoach = user.userRole === "Coach" && user.teamAffiliation && game.gameStatus === "Planned";
  const isReferee = user.userRole === "Referee" && user.id === game.refereeId && game.gameStatus == "Signed";

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (accepted === null) return;

    setSaving(true);
    setSaveError(null);

    try {
      if (isCoach) {
        await gameService.submitCoachResponse(game.id, accepted);
      } else if (isReferee) {
        await gameService.submitRefereeResponse(game.id, accepted);
      }
      setSubmitted(true);
    } catch (err) {
      setSaveError(err.message);
    } finally {
      setSaving(false);
    }
  };

  if (!isCoach && !isReferee) return null;
  if (submitted) return <p className="text-green-600">Ditt svar har sparats.</p>;

  return (
    <form onSubmit={handleSubmit} className="mt-6 p-4 rounded bg-gray-200">
      <h2 className="text-lg font-semibold mb-2">Godkänn matchöverenskommelsen</h2>
      <div className="space-y-2">
        <label className="block">
          <input
            type="radio"
            name="response"
            value="accept"
            onChange={() => setAccepted(true)}
            className="mr-2"
          />
          Jag accepterar matchöverenskommelsen
        </label>
        <label className="block">
          <input
            type="radio"
            name="response"
            value="reject"
            onChange={() => setAccepted(false)}
            className="mr-2"
          />
          Jag avböjer matchöverenskommelsen
        </label>
      </div>
      <button
        type="submit"
        disabled={accepted === null || saving}
        className="mt-4 px-4 py-2 bg-blue-600 text-white rounded disabled:opacity-50"
      >
        {saving ? "Sparar..." : "Spara"}
      </button>
      {saveError && <p className="text-red-600 mt-2">{saveError}</p>}
    </form>
  );
};

export default GameResponse;
