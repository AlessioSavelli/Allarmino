Module macro
    Public Sub DirectoryCopy(ByVal sourceDirName As String, ByVal destDirName As String, Optional ByVal copySubDirs As Boolean = True)
        Dim dir As New IO.DirectoryInfo(sourceDirName)
        If Not dir.Exists Then
            Throw New IO.DirectoryNotFoundException("Source directory does not exist or could not be found: " & sourceDirName)
        End If
        Dim dirs As IO.DirectoryInfo() = dir.GetDirectories
        IO.Directory.CreateDirectory(destDirName)
        Dim files As IO.FileInfo() = dir.GetFiles()
        For Each file As IO.FileInfo In files
            Dim temppath As String = IO.Path.Combine(destDirName, file.Name)
            file.CopyTo(temppath, False)
        Next
        If copySubDirs Then
            For Each subdir As IO.DirectoryInfo In dirs
                Dim temppath As String = IO.Path.Combine(destDirName, subdir.Name)
                DirectoryCopy(subdir.FullName, temppath, copySubDirs)
            Next
        End If
    End Sub
End Module
