import { useParams } from 'react-router-dom'

export default function DetailsPage() {
  const { type, id } = useParams()

  return (
    <section>
      <h2>Details</h2>
      <p>Type: {type}</p>
      <p>ID: {id}</p>
    </section>
  )
}
