import type { Metadata } from 'next'
import '../styles/globals.css'

export const metadata: Metadata = {
  title: 'BossSO',
  description: 'BossSO ERP System',
  icons: { icon: '/boss-icon.svg' },
}

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body className="min-h-screen bg-[#f0f0f0]">
        {children}
      </body>
    </html>
  )
}
