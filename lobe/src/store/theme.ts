import create from 'zustand';

interface ThemeState {
    themeMode: 'light' | 'dark' | 'auto';
    toggleTheme: (model: 'light' | 'dark' | 'auto') => void;
}

const useThemeStore = create<ThemeState>((set) => ({
    themeMode: localStorage.getItem('themeMode') as 'light' | 'dark' | 'auto' || 'auto',
    toggleTheme: (model: 'light' | 'dark' | 'auto') => {
        localStorage.setItem('themeMode', model);
        set({ themeMode: model });
    },
}));

export default useThemeStore;
