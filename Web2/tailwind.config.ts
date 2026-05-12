import type { Config } from 'tailwindcss'

const config: Config = {
  content: ['./src/**/*.{js,ts,jsx,tsx,mdx}'],
  theme: {
    extend: {
      colors: {
        bg: {
          base:    '#FAFAFA',
          surface: '#FFFFFF',
          muted:   '#F4F4F5',
        },
        border: {
          DEFAULT: '#E4E4E7',
          strong:  '#D4D4D8',
        },
        txt: {
          primary:   '#18181B',
          secondary: '#52525B',
          tertiary:  '#A1A1AA',
        },
        brand: {
          50:  '#EEF2FF',
          100: '#E0E7FF',
          500: '#6366F1',
          600: '#4F46E5',
          700: '#4338CA',
        },
        success: '#10B981',
        warning: '#F59E0B',
        danger:  '#EF4444',
      },
      fontFamily: {
        sans: ['var(--font-inter)', '-apple-system', 'system-ui', 'sans-serif'],
      },
      boxShadow: {
        'card':   '0 1px 2px 0 rgb(0 0 0 / 0.04), 0 1px 3px 0 rgb(0 0 0 / 0.06)',
        'card-h': '0 4px 6px -1px rgb(0 0 0 / 0.06), 0 2px 4px -2px rgb(0 0 0 / 0.05)',
        'pop':    '0 10px 15px -3px rgb(0 0 0 / 0.08), 0 4px 6px -4px rgb(0 0 0 / 0.06)',
      },
      borderRadius: {
        'xl': '12px',
        '2xl': '16px',
      },
    },
  },
  plugins: [],
}

export default config
