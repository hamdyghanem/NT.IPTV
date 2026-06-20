import { useParams } from 'react-router-dom'

export default function PlayerPage() {
  const { type, id } = useParams()

  return (
    <section>
      <h2>Player</h2>
      <p>Type: {type}</p>
      <p>ID: {id}</p>
    </section>
  )
}
