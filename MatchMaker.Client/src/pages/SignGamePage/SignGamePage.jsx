import { useParams, useLocation } from "react-router-dom";
import useGameData from "../../hooks/useGameData";
import { format, parseISO } from "date-fns";
import { sv } from "date-fns/locale";
import { useAuth } from "../../context/AuthContext/AuthContext";
import gameService from "../../services/gameService";

const SignGamePage = () => {
  const { id } = useParams();
  const location = useLocation();
  const { game, homeTeam, awayTeam, loading, error } = useGameData(id, location);
  const { user } = useAuth();
  const [isSubmitting, setIsSubmitting] = React.useState(false);
  const [submitError, setSubmitError] = React.useState(null);
  const [submitSuccess, setSubmitSuccess] = React.useState(null);

  const handleResponse = async (accepted) => {
    setIsSubmitting(true);
    setSubmitError(null);
    setSubmitSuccess(null);

    try {
      let response;
      if (user.role === 'Coach') {
        response = await gameService.submitCoachResponse(id, accepted);
      } else if (user.role === 'Referee') {
        response = await gameService.submitRefereeResponse(id, accepted);
      } else {
        throw new Error('Only coaches or referees can sign games');
      }
        setSubmitSuccess(`Game ${accepted ? 'accepted' : 'rejected'} successfully!`);
        // You might want to refresh the game data here
      } catch (err) {
      setSubmitError(err.response?.data?.message || err.message);
    } finally {
      setIsSubmitting(false);
    }
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error loading game data.</p>;

  // Check if the current user is the coach or referee for this game
  const isCoach = user.role === 'Coach' && (user.teamId === homeTeam._id || user.teamId === awayTeam._id);
  const isReferee = user.role === 'Referee' && user.id === game.refereeId;
  
  // Check if the user has already signed
  const hasSigned = isCoach ? game.isCoachSigned : (isReferee ? game.isRefereeSigned : false);

  return (
    <div className="p-4 max-w-3xl mx-auto bg-white shadow rounded">
      {/* Existing game details */}
      {/* ... (keep all your existing game display code) ... */}

      {/* Signature section */}
      {(isCoach || isReferee) && (
        <div className="mt-8 p-4 border-t">
          {submitSuccess && (
            <div className="mb-4 p-2 bg-green-100 text-green-800 rounded">
              {submitSuccess}
            </div>
          )}
          {submitError && (
            <div className="mb-4 p-2 bg-red-100 text-red-800 rounded">
              {submitError}
            </div>
          )}

          {hasSigned ? (
            <p className="text-green-600 font-medium">
              You have already accepted this game on{' '}
              {format(
                parseISO(isCoach ? game.coachSignedDate : game.refereeSignedDate),
                "PPPP 'at' p",
                { locale: sv }
              )}
            </p>
          ) : (
            <div className="flex justify-center space-x-4">
              <button
                onClick={() => handleResponse(true)}
                disabled={isSubmitting}
                className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600 disabled:bg-green-300"
              >
                {isSubmitting ? 'Processing...' : 'Accept Game'}
              </button>
              <button
                onClick={() => handleResponse(false)}
                disabled={isSubmitting}
                className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600 disabled:bg-red-300"
              >
                {isSubmitting ? 'Processing...' : 'Reject Game'}
              </button>
            </div>
          )}

          {isCoach && game.isCoachSigned && !game.isRefereeSigned && (
            <p className="mt-2 text-gray-600">
              Waiting for referee to accept the game...
            </p>
          )}
          {isReferee && game.isRefereeSigned && !game.isCoachSigned && (
            <p className="mt-2 text-gray-600">
              Waiting for coach to accept the game...
            </p>
          )}
          {game.gameStatus === 'Booked' && (
            <p className="mt-2 text-green-600 font-medium">
              Game is confirmed and booked!
            </p>
          )}
        </div>
      )}
    </div>
  );
};

export default SignGamePage;