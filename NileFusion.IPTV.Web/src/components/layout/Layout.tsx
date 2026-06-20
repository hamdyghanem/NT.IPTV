import { Outlet, NavLink } from 'react-router-dom'

const links = [
  { to: '/', label: 'Home' },
  { to: '/browse', label: 'Browse' },
  { to: '/settings', label: 'Settings' },
]

export default function Layout() {
  return (
    <div style={{ minHeight: '100vh', background: '#0f1117', color: '#f5f5f5' }}>
      <header style={{ padding: '1rem 1.5rem', borderBottom: '1px solid #222' }}>
        <nav style={{ display: 'flex', gap: '1rem' }}>
          {links.map((link) => (
            <NavLink
              key={link.to}
              to={link.to}
              style={({ isActive }) => ({
                color: isActive ? '#fff' : '#9ca3af',
                textDecoration: 'none',
                fontWeight: 600,
              })}
            >
              {link.label}
            </NavLink>
          ))}
        </nav>
      </header>
      <main style={{ padding: '1.5rem' }}>
        <Outlet />
      </main>
    </div>
  )
}
