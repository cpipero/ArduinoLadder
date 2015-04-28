import java.io.File;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
import java.util.regex.Matcher;
import java.util.regex.Pattern;


public class Main {

    public static final String patternReplace = "Version=\"\\d+.\\d+.\\d+.\\d+\"";

    public static void main(String[] args) throws IOException {

        if (args.length < 2){
            System.out.println("your need at least one file name as argument");
            return;
        }

        Path pathSource = Paths.get(args[0]);

        String sVersion = getVersionForReplace(pathSource);

        if (sVersion.isEmpty()){
            System.out.println("Can't found Version");
            return;
        }

        String sWillReplaced = "Version=" + sVersion;

        for (int i = 1; i < args.length; i++){
            Path path = Paths.get(args[i]);
            byte[] bytes = Files.readAllBytes(path);
            String sFileContent = new String(bytes, StandardCharsets.UTF_8);
            Pattern pattern = Pattern.compile(patternReplace);
            Matcher matcher = pattern.matcher(sFileContent);
            String sNewFileContent = matcher.replaceFirst(sWillReplaced);
            Files.write(path, sNewFileContent.getBytes(StandardCharsets.UTF_8));
            System.out.println("File: " + path + " done!");
        }
    }

    public static String getVersionForReplace(Path path) throws IOException {

        String sVersion = null;

        final String patternReplaceIn = "\"\\d+.\\d+.\\d+.\\d+\"";

        final String patternForReplace = "AssemblyVersion \\(\"\\d+.\\d+.\\d+.\\d+\"";

        byte[] bytesSource = Files.readAllBytes(path);
        // content file where we get version
        String contentSource = new String(bytesSource, StandardCharsets.UTF_8);

        Pattern pattern = Pattern.compile(patternForReplace);
        Matcher matcher = pattern.matcher(contentSource);

        if (matcher.find()){
            String sBeforeFind = contentSource.substring(matcher.start(), matcher.end());
            Pattern patternVersion = Pattern.compile(patternReplaceIn);
            Matcher matcher1 = patternVersion.matcher(sBeforeFind);
            if (matcher1.find()){
                sVersion = sBeforeFind.substring(matcher1.start(), matcher1.end());
                System.out.println("In file: " + path + " version: " + sVersion.substring(1, sVersion.length() - 1));
            }
        }

        return sVersion;
    }

}
