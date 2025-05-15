import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import gameService from '../services/gameService';

const EditGamePage = () => {
  const { gameId } = useParams();
  const navigate = useNavigate();

  const [game, setGame] = useState(null);
  const [formData, setFormData] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchGame = async () => {
      try {
        const gameData = await gameService.getGameById(gameId);
        setGame(gameData);
        setFormData(gameData);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    fetchGame();
  }, [gameId]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const isEditable = (field) => {
    if (game.status === 'draft') return true;
    if (game.status === 'planned') return ['refereeId', 'condition'].includes(field);
    if (game.status === 'signed') return field === 'refereeId';
    return false;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await gameService.updateGame(formData);
      navigate('/'); 
    } catch (err) {
      setError(err.message);
    }
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p className="text-red-500">Error: {error}</p>;

  return (
    <div className="max-w-2xl mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Edit Game</h1>
      <form onSubmit={handleSubmit} className="space-y-4">

        <div>
          <label className="block">Title</label>
          <input
            name="title"
            value={formData.title || ''}
            onChange={handleChange}
            disabled={!isEditable('title')}
            className="w-full p-2 border rounded"
          />
        </div>

        <div>
          <label className="block">Date</label>
          <input
            type="datetime-local"
            name="date"
            value={formData.date?.slice(0, 16) || ''}
            onChange={handleChange}
            disabled={!isEditable('date')}
            className="w-full p-2 border rounded"
          />
        </div>

        <div>
          <label className="block">Home Team</label>
          <input
            name="homeTeamId"
            value={formData.homeTeamId || ''}
            onChange={handleChange}
            disabled={!isEditable('homeTeamId')}
            className="w-full p-2 border rounded"
          />
        </div>

        <div>
          <label className="block">Away Team</label>
          <input
            name="awayTeamId"
            value={formData.awayTeamId || ''}
            onChange={handleChange}
            disabled={!isEditable('awayTeamId')}
            className="w-full p-2 border rounded"
          />
        </div>

        <div>
          <label className="block">Condition</label>
          <input
            name="condition"
            value={formData.condition || ''}
            onChange={handleChange}
            disabled={!isEditable('condition')}
            className="w-full p-2 border rounded"
          />
        </div>

        <div>
          <label className="block">Referee ID</label>
          <input
            name="refereeId"
            value={formData.refereeId || ''}
            onChange={handleChange}
            disabled={!isEditable('refereeId')}
            className="w-full p-2 border rounded"
          />
        </div>

        <button
          type="submit"
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          Save Changes
        </button>
      </form>
    </div>
  );
};

export default EditGamePage;
